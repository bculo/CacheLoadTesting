Solution contains examples of load testing of Cache.API with NBomber tool.

In solution there are 2 separated projects:
  Cache.API - REST API that uses IDistributedCache interface for storing and fetching items from cache (in this case project uses in memory cache)
  NBomber.Testing - Console application that uses NBomber package for Cache.API load testing
