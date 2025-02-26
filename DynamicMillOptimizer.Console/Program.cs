using DynamicMillOptimizer.Console.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace DynamicMillOptimizer.Console;

internal static class Program
{
    private static IServiceCollection? _services;

    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(RegisterServices)
            .Build();

        await BuildApp().RunAsync(args);

        await host.StopAsync();
    }

    private static void RegisterServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        _services = services;
        // Register dependencies here for dependency injection.
    }
    
    private static ICommandApp BuildApp()
    {
        var app = new CommandApp<DefaultCommand>(new TypeRegistrar(_services));
        app.Configure(config =>
        {
            config.AddCommand<OptimizeFileCommand>("optimize")
                .WithDescription("Optimizes a dynamic milling file.")
                .WithExample("optimize", @"""C:\Users\John\Downloads\dynamic-mill-sample.txt""");
        });
        return app;
    }
}