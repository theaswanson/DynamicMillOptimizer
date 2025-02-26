namespace DynamicMillOptimizer.Console.Commands;

public class EndCannedCycleCommand : ICommand
{
    public string Text { get; }

    public EndCannedCycleCommand(string text)
    {
        Text = text;
    }
    
    public bool CanBeOptimizedWith(ICommand command) => false;
}