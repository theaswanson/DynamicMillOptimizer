using DynamicMillOptimizer.Console.Commands;
using DynamicMillOptimizer.Console.Commands.Optimizers;

namespace DynamicMillOptimizer.Console;

public class FileOptimizer
{
    private readonly CommandParser _commandParser;
    private readonly ICommandOptimizer<SingleAxisCommand> _singleAxisOptimizer;

    public FileOptimizer(CommandParser commandParser, ICommandOptimizer<SingleAxisCommand> singleAxisOptimizer)
    {
        _commandParser = commandParser;
        _singleAxisOptimizer = singleAxisOptimizer;
    }
    
    /// <summary>
    /// Optimizes lines from a milling file.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public string[] Optimize(string[] lines)
    {
        if (lines.Length == 0)
        {
            return [];
        }
        
        List<string> output = [];
        var context = new OptimizationContext();

        foreach (var currentLine in lines)
        {
            var command = _commandParser.Parse(currentLine);

            if (command is not UnhandledCommand unhandledCommand)
            {
                if (!context.CanAdd(command))
                {
                    OptimizeCurrentSetAndWriteToOutput(context, output);
                }

                context.Add(command);
            }
            else
            {
                OptimizeCurrentSetAndWriteToOutput(context, output);
                WriteCommandToOutput(unhandledCommand, output);
            }
        }
        
        // If we reach the end of the file while handling a set of optimizable lines,
        // we need to optimize them and write them to the output before we finish.
        OptimizeCurrentSetAndWriteToOutput(context, output);

        return output.ToArray();
    }

    private void OptimizeCurrentSetAndWriteToOutput(OptimizationContext context, List<string> output)
    {
        if (context.IsEmpty)
        {
            return;
        }

        var optimizedCommands = context.OptimizeAndClear(
            commandsToOptimize => commandsToOptimize.First() switch
            {
                SingleAxisCommand => _singleAxisOptimizer.Optimize(commandsToOptimize.OfType<SingleAxisCommand>().ToArray())
                    .ToArray<ICommand>(),
                _ => throw new Exception("Unrecognized command.")
            });

        WriteCommandsToOutput(optimizedCommands, output);
    }
    
    private static void WriteCommandToOutput(ICommand command, List<string> output) => output.Add(command.Text);
    private static void WriteCommandsToOutput(IEnumerable<ICommand> commands, List<string> output) => output.AddRange(commands.Select(c => c.Text));
}