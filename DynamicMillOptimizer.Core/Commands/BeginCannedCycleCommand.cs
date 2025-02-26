namespace DynamicMillOptimizer.Core.Commands;

public class BeginCannedCycleCommand : ICommand
{
    public string Text { get; }

    public BeginCannedCycleCommand(string text)
    {
        Text = text;
    }
    
    public bool CanBeOptimizedWith(ICommand command) => false;
}