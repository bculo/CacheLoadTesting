using Cache.API.Models;
using Cache.API.Persistence;
using Cache.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;

namespace Cache.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheScenarioTwoController : ControllerBase
    {
        private const string CACHE_KEY = "TEST";

        private static AsyncDuplicateNormalDictLock _lock = new AsyncDuplicateNormalDictLock();

        private readonly TestDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheScenarioOneController> _logger;

        public CacheScenarioTwoController(IDistributedCache cache,
            ILogger<CacheScenarioOneController> logger,
            TestDbContext context)
        {
            _cache = cache;
            _logger = logger;
            _context = context;
        }

        [HttpGet("Clean")]
        public async Task<IActionResult> ClearCacheNoLockScenario()
        {
            await _cache.RemoveAsync(CACHE_KEY);
            return NoContent();
        }

        [HttpGet("Fetch")]
        public async Task<IActionResult> NoLockScenario()
        {
            var cacheItem = _cache.GetString(CACHE_KEY);
            if (cacheItem is not null)
            {
                return Ok();
            }

            var cacheResponse = new CacheResponse { };
            using (var releaser = await _lock.LockAsync(CACHE_KEY))
            {
                cacheItem = _cache.GetString(CACHE_KEY);

                if(cacheItem is null)
                {
                    _logger.LogInformation("Executing expensive operation!!!");
                    var dbItem = await _context.Users.SingleAsync(i => i.Key == CACHE_KEY);
                    await _cache.SetStringAsync(CACHE_KEY, dbItem.Value);
                    cacheResponse = new CacheResponse { Value = dbItem.Value, ValueFromCache = false };
                }
                else
                {
                    cacheResponse = new CacheResponse { Value = cacheItem, ValueFromCache = true };
                }
            }

            if(cacheResponse.ValueFromCache)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
