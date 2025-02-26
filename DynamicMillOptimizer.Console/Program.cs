using DynamicMillOptimizer.Console.Commands;
using DynamicMillOptimizer.Core;
using DynamicMillOptimizer.Core.Commands;
using DynamicMillOptimizer.Core.Commands.Optimizers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace DynamicMillOptimizer.Console;

internal static class Program
{
    static IServiceCollection _services;

    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(RegisterServices)
            .Build();

        BuildApp().Run(args);

        await host.StopAsync();
    }
    
    static void RegisterServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        _services = services;
        // Register dependencies here for dependency injection.
    }
    
    private static CommandApp BuildApp()
    {
        var app = new CommandApp(new TypeRegistrar(_services));
        app.Configure(config =>
        {
            config.AddCommand<OptimizeFileCommand>("optimize");
        });
        return app;
    }
}