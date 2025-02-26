namespace DynamicMillOptimizer.Core.Commands.Optimizers;

public interface ICommandOptimizer<T> where T : ICommand
{
    T[] Optimize(T[] commands);
}