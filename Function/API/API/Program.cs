using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
     .ConfigureServices(services =>
     {
         services.AddLogging();

         // Add HttpClient
         services.AddHttpClient();
     })
    .Build();

await host.RunAsync();
