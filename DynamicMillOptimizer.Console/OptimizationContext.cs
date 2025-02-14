using DynamicMillOptimizer.Console.Commands;

namespace DynamicMillOptimizer.Console;

/// <summary>
/// Tracks a single set of commands that need to be optimized.
/// </summary>
public class OptimizationContext
{
    public bool IsEmpty => _commandsToOptimize.Count == 0;

    private readonly List<ICommand> _commandsToOptimize = [];
    
    public void Add(ICommand command) => _commandsToOptimize.Add(command);

    /// <summary>
    /// Determines if the command can be added to the current set. If the current set is empty, returns true.
    /// <para />
    /// If the command cannot be added, then the current set of commands needs to be optimized with <see cref="OptimizeAndClear"/>
    /// first.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool CanAdd(ICommand command) => IsEmpty || _commandsToOptimize[0].CanBeOptimizedWith(command);

    /// <summary>
    /// Optimize the current set of commands, after which the set will be cleared.
    /// </summary>
    /// <param name="optimizer"></param>
    /// <returns></returns>
    public IEnumerable<ICommand> OptimizeAndClear(Func<IEnumerable<ICommand>, IEnumerable<ICommand>> optimizer)
    {
        if (IsEmpty)
        {
            return [];
        }
        
        var optimized = optimizer(_commandsToOptimize);
        
        _commandsToOptimize.Clear();
        
        return optimized;
    }
}