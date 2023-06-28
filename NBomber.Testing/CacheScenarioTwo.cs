using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBomber.Testing
{
    public class CacheScenarioTwo : ICacheScenarioData
    {
        public string ScenarioIdentifier => "cache_scenario_two";

        public string CleanCacheUri => "https://localhost:7279/api/CacheScenarioTwo/Clean";

        public string FetchFromCacheUri => "https://localhost:7279/api/CacheScenarioTwo/Fetch";
    }
}
