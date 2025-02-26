using System.Text.RegularExpressions;

namespace DynamicMillOptimizer.Core.Commands;

public class CommandParser
{
    /// <summary>
    /// Matches lines in the format X123.456 or Y123.456
    /// </summary>
    private readonly Regex _singleAxisCommandPattern = new(@"^([XY])-?(\d*\.\d*)$");
    private readonly Regex _beginCannedCycleCommandPattern = new("G(?:73|74|76|77|81|82|83|84|85|86|89)");
    private readonly Regex _endCannedCycleCommandPattern = new("G80");

    public ICommand Parse(string command) =>
        TryParseSingleAxisCommand(command) as ICommand ??
        TryParseBeginCannedCycleCommand(command) as ICommand ??
        TryParseEndCannedCycleCommand(command) as ICommand ??
        new UnhandledCommand(command);

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
    
    private BeginCannedCycleCommand? TryParseBeginCannedCycleCommand(string command) =>
        _beginCannedCycleCommandPattern.Match(command).Success ? new BeginCannedCycleCommand(command) : null;
    
    private EndCannedCycleCommand? TryParseEndCannedCycleCommand(string command) =>
        _endCannedCycleCommandPattern.Match(command).Success ? new EndCannedCycleCommand(command) : null;
}