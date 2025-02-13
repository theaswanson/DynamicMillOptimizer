using System.Text.RegularExpressions;

namespace DynamicMillOptimizer.Console;

public class Optimizer
{
    /// <summary>
    /// Matches lines in the format X123.456 or Y123.456
    /// </summary>
    private readonly Regex _linePattern = new(@"^([XY])(\d*\.\d*)$");
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public string[] Optimize(string[] lines)
    {
        List<string> output = [];
        List<string> optimizedLinesBuffer = [];
        string? optimizedAxis = null;
        
        foreach (var currentLine in lines)
        {
            // Check if the current line can be optimized
            var match = _linePattern.Match(currentLine);

            if (!match.Success)
            {
                // Current line cannot be optimized.
                
                WriteAnyOptimizedLinesToOutput(optimizedLinesBuffer, output);
                
                output.Add(currentLine);
                continue;
            }
            
            // Current line can be optimized.

            // Determine if the current line is for X or Y
            var axis = match.Groups[1].Value;

            if (axis == optimizedAxis)
            {
                // The line matches the existing group of optimized lines. Add it to the list.
                optimizedLinesBuffer.Add(currentLine);
            }
            else
            {
                // The line matches the existing group of optimized lines. Write the current buffer to the output, clear it, and add the current line to the new group of optimized lines.
                WriteAnyOptimizedLinesToOutput(optimizedLinesBuffer, output);
                optimizedAxis = axis;
                optimizedLinesBuffer.Add(currentLine);
            }
        }

        return output.ToArray();
    }

    private static void WriteAnyOptimizedLinesToOutput(List<string> optimizedLinesBuffer, List<string> output)
    {
        if (optimizedLinesBuffer.Any())
        {
            // There is a group of lines that needs to be optimized and copied to the output list.
            if (optimizedLinesBuffer.Count == 1)
            {
                output.Add(optimizedLinesBuffer.Single());
            }
            else
            {
                // We'll assume that the tool is moving in a straight direction.
                // In this case, we only want the first and last coordinates in the group.
                output.AddRange([optimizedLinesBuffer.First(), optimizedLinesBuffer.Last()]);
            }
                    
            // Clear the buffer in preparation for the next group of lines we encounter.
            optimizedLinesBuffer.Clear();
        }
    }
}