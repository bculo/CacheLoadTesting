using NBomber.Contracts.Stats;
using NBomber.CSharp;
using NBomber.Testing;

Console.WriteLine("Hit any button to start testing");
Console.ReadLine();
Console.WriteLine("STARTING");

ICacheScenarioData cacheScenarioData = new CacheScenarioThree();
HttpClient client = new HttpClient();

var scenario = Scenario.Create(cacheScenarioData.ScenarioIdentifier, async context =>
    {
        var response = await client.GetAsync(cacheScenarioData.FetchFromCacheUri);  
        if(!response.IsSuccessStatusCode)
        {
            return Response.Fail(response.StatusCode);
        }
        return Response.Ok();
    })
    .WithoutWarmUp()
    .WithInit(async context => await client.GetAsync(cacheScenarioData.CleanCacheUri))
    .WithClean(context =>
    {
        client?.Dispose();
        return Task.CompletedTask;
    })
    .WithLoadSimulations(Simulation.Inject(rate: 2000, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(15)));

NBomberRunner
  .RegisterScenarios(scenario)
  .WithReportFileName(cacheScenarioData.ScenarioIdentifier)
  .WithReportFolder(cacheScenarioData.ScenarioIdentifier)
  .WithReportFormats(ReportFormat.Html)
  .Run();

Console.ReadLine();