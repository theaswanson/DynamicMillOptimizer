namespace DynamicMillOptimizer.Console.Commands;

public class SingleAxisCommand : ICommand
{
    public string Text { get; }
    public Axis Axis { get; }
    public decimal Point { get; }

    public SingleAxisCommand(string text, Axis axis, decimal point)
    {
        Text = text;
        Axis = axis;
        Point = point;
    }

    public bool CanBeOptimizedWith(ICommand command) =>
        command is SingleAxisCommand singleAxisCommand && singleAxisCommand.Axis == Axis;
}