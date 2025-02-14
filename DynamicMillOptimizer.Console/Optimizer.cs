using System.Text.RegularExpressions;

namespace DynamicMillOptimizer.Console;

public class Optimizer
{
    /// <summary>
    /// Matches lines in the format X123.456 or Y123.456
    /// </summary>
    private readonly Regex _optimizableLinePattern = new(@"^([XY])-?(\d*\.\d*)$");

    public string[] Optimize(string[] lines)
    {
        List<string> output = [];
        var optimizedLinesGroup = new OptimizedLinesGroup();

        foreach (var currentLine in lines)
        {
            var match = _optimizableLinePattern.Match(currentLine);

            var lineCanBeOptimized = match.Success;

            if (lineCanBeOptimized)
            {
                HandleOptimizableLine(optimizedLinesGroup, currentLine, match, output);
            }
            else
            {
                HandleUnoptimizableLine(optimizedLinesGroup, output, currentLine);
            }
        }
        
        MoveBufferToOutput(optimizedLinesGroup, output);

        return output.ToArray();
    }

    private static void HandleOptimizableLine(OptimizedLinesGroup optimizedLinesGroup, string currentLine, Match match,
        List<string> output)
    {
        var groupExists = optimizedLinesGroup.Buffer.Count > 0;
        
        if (groupExists && !CurrentLineBelongsToExistingGroup())
        {
            MoveBufferToOutput(optimizedLinesGroup, output);
        }

        optimizedLinesGroup.Add(currentLine);
        return;

        bool CurrentLineBelongsToExistingGroup()
        {
            var axisOfCurrentLine = match.Groups[1].Value;

            return axisOfCurrentLine == optimizedLinesGroup.Axis;
        }
    }

    private static void HandleUnoptimizableLine(OptimizedLinesGroup optimizedLinesGroup, List<string> output, string currentLine)
    {
        MoveBufferToOutput(optimizedLinesGroup, output);
        output.Add(currentLine);
    }

    private static void MoveBufferToOutput(OptimizedLinesGroup optimizedLinesGroup, List<string> output)
    {
        var buffer = optimizedLinesGroup.Buffer;

        if (!buffer.Any())
        {
            return;
        }
        
        // There is a group of lines that needs to be optimized and copied to the output list.
        if (buffer.Count == 1)
        {
            output.Add(buffer.Single());
        }
        else
        {
            // We'll assume that the tool is moving in a straight direction.
            // In this case, we only want the first and last coordinates in the group.
            output.AddRange([buffer.First(), buffer.Last()]);
        }

        // Clear the buffer in preparation for the next group of lines we encounter.
        optimizedLinesGroup.Clear();
    }
}