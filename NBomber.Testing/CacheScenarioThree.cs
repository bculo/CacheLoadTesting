using NBomber.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBomber.Testing
{
    public class CacheScenarioThree : ICacheScenarioData
    {
        public string ScenarioIdentifier => "cache_scenario_three";

        public string CleanCacheUri => "https://localhost:7279/api/CacheScenarioThree/Clean";

        public string FetchFromCacheUri => "https://localhost:7279/api/CacheScenarioThree/Fetch";
    }
}
