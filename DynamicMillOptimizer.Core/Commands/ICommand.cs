namespace DynamicMillOptimizer.Core.Commands;

public interface ICommand
{
    /// <summary>
    /// The raw command text from the milling file.
    /// </summary>
    public string Text { get; }
    
    bool CanBeOptimizedWith(ICommand command);
}