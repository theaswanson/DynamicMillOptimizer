using DynamicMillOptimizer.Core.Commands;

namespace DynamicMillOptimizer.Core;

/// <summary>
/// Tracks a single set of commands that need to be optimized.
/// </summary>
public class OptimizationContext
{
    public bool IsEmpty => _commandsToOptimize.Count == 0;

    private readonly List<ICommand> _commandsToOptimize = [];

    private bool _paused;
    /// <summary>
    /// Determines if optimization is currently paused. When paused, no commands will be added to the current set.
    /// </summary>
    public bool IsPaused => _paused;
    public void Pause() => _paused = true;
    public void Unpause() => _paused = false;
    
    public void Add(ICommand command) => _commandsToOptimize.Add(command);

    /// <summary>
    /// Determines if the command can be added to the current set. If the current set is empty, returns true.
    /// <para />
    /// If the command cannot be added, then the current set of commands needs to be optimized with <see cref="OptimizeAndClear"/>
    /// first.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool CanAdd(ICommand command) => IsEmpty || (!IsPaused && _commandsToOptimize[0].CanBeOptimizedWith(command));

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