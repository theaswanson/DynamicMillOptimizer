using DynamicMillOptimizer.Core;
using DynamicMillOptimizer.Core.Commands;
using DynamicMillOptimizer.Core.Commands.Optimizers;
using Spectre.Console;

namespace DynamicMillOptimizer.Console;

public class FileOptimizerService
{
    public static async Task<OptimizationStatus> OptimizeAsync(string? filePath)
    {
        var (result, file) = FilePathValidator.Validate(filePath);

        if (result != FilePathValidationResult.Valid || file is null)
        {
            var errorMessage = result switch
            {
                FilePathValidationResult.InvalidFilePath => "Invalid file path.",
                FilePathValidationResult.FileNotFound => "File not found.",
                _ => "Something went wrong."
            };
            
            AnsiConsole.WriteLine($"{errorMessage} Exiting...");
            
            return OptimizationStatus.InvalidFile;
        }

        var lines = await ReadFileAsync(file);

        var optimizedLines = new FileOptimizer(new CommandParser(), new SingleAxisOptimizer()).Optimize(lines);

        var noLinesWereOptimized = optimizedLines.Length == lines.Length;
        
        if (noLinesWereOptimized)
        {
            AnsiConsole.WriteLine("File is already optimized. Exiting...");
            return OptimizationStatus.NoOptimizationNeeded;
        }

        AnsiConsole.WriteLine("Optimized from {0} lines to {1}.", lines.Length, optimizedLines.Length);

        await SaveOptimizedFileAsync(file, optimizedLines);

        return OptimizationStatus.Optimized;
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