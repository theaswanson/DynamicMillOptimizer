namespace DynamicMillOptimizer.Console;

public class OptimizedLinesGroup
{
    private readonly List<string> _buffer = [];
    public IReadOnlyList<string> Buffer => _buffer;
    public string? Axis => _buffer.FirstOrDefault()?.Substring(0, 1);

    public void Add(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            throw new Exception("Invalid line");
        }

        if (Axis != null)
        {
            var lineAxis = line.Substring(0, 1);

            if (!string.Equals(lineAxis, Axis, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception($"Line axis {lineAxis} does not match existing axis {Axis}.");
            }
        }
        
        _buffer.Add(line);
    }

    public void Clear() => _buffer.Clear();
}