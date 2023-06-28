using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBomber.Testing
{
    public interface ICacheScenarioData
    {
        string ScenarioIdentifier { get; }
        string CleanCacheUri { get; }
        string FetchFromCacheUri { get; }
    }
}
