using Cache.API.Models;
using Cache.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Cache.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheScenarioOneController : ControllerBase
    {
        private const string CACHE_KEY = "TEST";

        private readonly TestDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheScenarioOneController> _logger;

        public CacheScenarioOneController(IDistributedCache cache,
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

            if(cacheItem is not null)
            { 
                return Ok(new CacheResponse
                {
                    Value = cacheItem,
                    ValueFromCache = true
                });
            }

            _logger.LogWarning("EXECUTING EXPENSIVE OPERATION");
            var item = _context.Users.Single(i => i.Key == CACHE_KEY);
            await _cache.SetStringAsync(CACHE_KEY, item.Value);

            return BadRequest(new CacheResponse
            {
                Value = item.Value,
                ValueFromCache = false
            });
        }  
    }
}
