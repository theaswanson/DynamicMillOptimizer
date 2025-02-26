namespace DynamicMillOptimizer.Core.Commands.Optimizers;

public class SingleAxisOptimizer : ICommandOptimizer<SingleAxisCommand>
{
    public SingleAxisCommand[] Optimize(SingleAxisCommand[] commands) =>
        commands.Length > 1
            ? [commands.First(), commands.Last()]
            : commands;
}