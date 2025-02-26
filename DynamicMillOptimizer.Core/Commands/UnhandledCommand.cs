namespace DynamicMillOptimizer.Core.Commands;

public class UnhandledCommand : ICommand
{
    public string Text { get; }

    public UnhandledCommand(string text)
    {
        Text = text;
    }
    
    public bool CanBeOptimizedWith(ICommand command) => false;
}