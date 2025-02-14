namespace DynamicMillOptimizer.Console.Commands.Optimizers;

public interface ICommandOptimizer<T> where T : ICommand
{
    T[] Optimize(T[] commands);
}