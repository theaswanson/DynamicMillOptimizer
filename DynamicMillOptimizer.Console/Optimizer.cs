using System.Text.RegularExpressions;

namespace DynamicMillOptimizer.Console;

public class Optimizer
{
    /// <summary>
    /// Matches lines in the format X123.456 or Y123.456
    /// </summary>
    private readonly Regex _linePattern = new(@"^([XY])-?(\d*\.\d*)$");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public string[] Optimize(string[] lines)
    {
        List<string> output = [];
        var optimizedLinesGroup = new OptimizedLinesGroup();

        foreach (var currentLine in lines)
        {
            // Check if the current line can be optimized
            var match = _linePattern.Match(currentLine);

            if (!match.Success)
            {
                // Current line cannot be optimized.

                WriteAnyOptimizedLinesToOutput(optimizedLinesGroup, output);

                output.Add(currentLine);
                continue;
            }

            // Current line can be optimized.

            // Determine if the current line is for X or Y
            var axis = match.Groups[1].Value;

            if (optimizedLinesGroup.Axis is null)
            {
                optimizedLinesGroup.Add(currentLine);
            }

            if (axis == optimizedLinesGroup.Axis)
            {
                // The line matches the existing group of optimized lines. Add it to the list.
                optimizedLinesGroup.Add(currentLine);
            }
            else
            {
                // The line matches the existing group of optimized lines. Write the current buffer to the output, clear it, and add the current line to the new group of optimized lines.
                WriteAnyOptimizedLinesToOutput(optimizedLinesGroup, output);
                optimizedLinesGroup.Add(currentLine);
            }
        }
        
        WriteAnyOptimizedLinesToOutput(optimizedLinesGroup, output);

        return output.ToArray();
    }

    private static void WriteAnyOptimizedLinesToOutput(OptimizedLinesGroup optimizedLinesGroup, List<string> output)
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