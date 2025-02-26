using System.ComponentModel;
using Spectre.Console.Cli;

namespace DynamicMillOptimizer.Console.Commands;

public class OptimizeFileCommand : AsyncCommand<OptimizeFileCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to the dynamic milling file to optimize.")]
        [CommandArgument(0, "<filepath>")]
        public string? FilePath { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var result = await FileOptimizerService.OptimizeAsync(settings.FilePath);

        return result switch
        {
            OptimizationStatus.Optimized or OptimizationStatus.NoOptimizationNeeded => 0,
            OptimizationStatus.InvalidFile => 1,
            _ => throw new NotImplementedException("Unknown optimization status.")
        };
    }
}