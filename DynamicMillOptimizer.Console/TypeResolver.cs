using Spectre.Console.Cli;

namespace DynamicMillOptimizer.Console;

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object? Resolve(Type? type) => type == null ? null : _provider.GetService(type);
}