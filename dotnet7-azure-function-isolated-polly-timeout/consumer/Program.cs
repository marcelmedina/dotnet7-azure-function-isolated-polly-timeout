using consumer.TypedHttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var currentDirectory = hostingContext.HostingEnvironment.ContentRootPath;

        config.SetBasePath(currentDirectory)
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        config.Build();
    })
    .ConfigureServices((services) =>
    {
        var timeoutPolicy = Policy
            .TimeoutAsync<HttpResponseMessage>(10, onTimeoutAsync: (_, timeSpan, _) =>
            {
                Console.Out.WriteLine($"### Timeout after {timeSpan.Seconds} seconds");

                return Task.CompletedTask;
            });

        services.AddHttpClient<DelayHttpClient>()
            .AddPolicyHandler(timeoutPolicy);
    })
    .Build();

host.Run();
