using System.ComponentModel;
using DynamicMillOptimizer.Core;
using DynamicMillOptimizer.Core.Commands;
using DynamicMillOptimizer.Core.Commands.Optimizers;
using Spectre.Console;
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
        var (isValid, file, errorMessage) = ValidateFile(settings.FilePath);

        if (!isValid || file is null)
        {
            errorMessage ??= "Something went wrong.";
            
            AnsiConsole.WriteLine($"{errorMessage} Exiting...");
            return 1;
        }

        var lines = await ReadFileAsync(file);

        var optimizedLines = new FileOptimizer(new CommandParser(), new SingleAxisOptimizer()).Optimize(lines);

        var noLinesWereOptimized = optimizedLines.Length == lines.Length;
        
        if (noLinesWereOptimized)
        {
            AnsiConsole.WriteLine("File is already optimized. Exiting...");
            return 0;
        }

        AnsiConsole.WriteLine("Optimized from {0} lines to {1}.", lines.Length, optimizedLines.Length);

        await SaveOptimizedFileAsync(file, optimizedLines);

        return 0;
    }

    private (bool IsValid, FileInfo? File, string? ErrorMessage) ValidateFile(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return (false, null, "Invalid file path.");
        }

        var filePathWithoutQuotes = filePath.Replace("\"", "");

        var file = new FileInfo(filePathWithoutQuotes);

        if (!file.Exists)
        {
            return (false, null, "File not found.");
        }

        return (true, file, null);
    }

    private static async Task<string[]> ReadFileAsync(FileInfo file)
    {
        AnsiConsole.WriteLine("Reading...");

        return await File.ReadAllLinesAsync(file.FullName);
    }
    
    private static async Task SaveOptimizedFileAsync(FileInfo file, string[] optimizedLines)
    {
        var optimizedFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-optimized.txt";
        var optimizedFilePath = Path.Combine(file.DirectoryName!, optimizedFileName);

        await File.WriteAllLinesAsync(optimizedFilePath, optimizedLines);

        AnsiConsole.WriteLine("Saved to file: {0}", optimizedFilePath);
    }
}