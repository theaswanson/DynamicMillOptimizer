using System.Text.RegularExpressions;

namespace DynamicMillOptimizer.Console.Commands;

public class CommandParser
{
    /// <summary>
    /// Matches lines in the format X123.456 or Y123.456
    /// </summary>
    private readonly Regex _singleAxisCommandPattern = new(@"^([XY])-?(\d*\.\d*)$");

    public ICommand Parse(string command) => TryParseSingleAxisCommand(command) as ICommand ?? new UnhandledCommand(command);

    private SingleAxisCommand? TryParseSingleAxisCommand(string command)
    {
        var match = _singleAxisCommandPattern.Match(command);

        if (!match.Success)
        {
            return null;
        }

        var axis = match.Groups[1].Value.ToUpper() switch
        {
            "X" => Axis.X,
            "Y" => Axis.Y,
            _ => throw new Exception("Invalid axis.")
        };

        var point = decimal.Parse(match.Groups[2].Value);

        return new SingleAxisCommand(command, axis, point);
    }
}