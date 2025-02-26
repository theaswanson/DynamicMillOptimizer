using Spectre.Console;
using Spectre.Console.Cli;

namespace DynamicMillOptimizer.Console.Commands;

public class DefaultCommand : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        OptimizationStatus? optimizationStatus = null;

        while (optimizationStatus is null or OptimizationStatus.InvalidFile)
        {
            AnsiConsole.Write("Path to file: ");
            var filePath = System.Console.ReadLine();

            optimizationStatus = await FileOptimizerService.OptimizeAsync(filePath);
        }

        return optimizationStatus switch
        {
            OptimizationStatus.Optimized or OptimizationStatus.NoOptimizationNeeded => 0,
            OptimizationStatus.InvalidFile => 1,
            _ => throw new NotImplementedException("Unknown optimization status.")
        };
    }
}