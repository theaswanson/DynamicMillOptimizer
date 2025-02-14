// See https://aka.ms/new-console-template for more information

using DynamicMillOptimizer.Console;
using DynamicMillOptimizer.Console.Commands;
using DynamicMillOptimizer.Console.Commands.Optimizers;

// Get input file
Console.Write("Path to file: ");
var input = Console.ReadLine();

if (string.IsNullOrWhiteSpace(input))
{
    Console.WriteLine("Invalid file path. Exiting...");
    return 1;
}

var filepath = input.Replace("\"", "");

var file = new FileInfo(filepath);

if (!file.Exists)
{
    Console.WriteLine("File not found. Exiting...");
    return 1;
}

// Read the file
Console.WriteLine("Reading...");

var lines = await File.ReadAllLinesAsync(file.FullName);

// Optimize it
var optimizedLines = new FileOptimizer(new CommandParser(), new SingleAxisOptimizer()).Optimize(lines);

if (optimizedLines.Length == lines.Length)
{
    Console.WriteLine("File is already optimized. Exiting...");
    return 0;
}

Console.WriteLine("Optimized from {0} lines to {1}.", lines.Length, optimizedLines.Length);

var optimizedFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-optimized.txt";
var optimizedFilePath = Path.Combine(file.DirectoryName, optimizedFileName);

// Save to new file
await File.WriteAllLinesAsync(optimizedFilePath, optimizedLines);

Console.WriteLine("Saved to file: {0}", optimizedFilePath);

return 0;