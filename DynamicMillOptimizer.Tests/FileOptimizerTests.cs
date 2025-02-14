using DynamicMillOptimizer.Console;
using DynamicMillOptimizer.Console.Commands;
using DynamicMillOptimizer.Console.Commands.Optimizers;
using FluentAssertions;

namespace DynamicMillOptimizer.Tests;

[TestFixture]
public class FileOptimizerTests
{
    private FileOptimizer _fileOptimizer;

    [SetUp]
    public void Setup()
    {
        _fileOptimizer = new FileOptimizer(new CommandParser(), new SingleAxisOptimizer());
    }

    [Test]
    public void OptimizesGroupsOfX()
    {
        string[] lines =
        [
            "X1.1",
            "X1.2",
            "X1.3",
            "X1.4",
            "X1.5",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "X1.1",
            "X1.5"
        ]);
    }
    
    [Test]
    public void OptimizesGroupsOfY()
    {
        string[] lines =
        [
            "Y1.1",
            "Y1.2",
            "Y1.3",
            "Y1.4",
            "Y1.5",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "Y1.1",
            "Y1.5"
        ]);
    }
    
    [Test]
    public void OptimizesGroupsOfXWithNegativeCoordinates()
    {
        string[] lines =
        [
            "X-1.1",
            "X-1.2",
            "X-1.3",
            "X-1.4",
            "X-1.5",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "X-1.1",
            "X-1.5"
        ]);
    }
    
    [Test]
    public void OptimizesGroupsOfYWithNegativeCoordinates()
    {
        string[] lines =
        [
            "Y-1.1",
            "Y-1.2",
            "Y-1.3",
            "Y-1.4",
            "Y-1.5",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "Y-1.1",
            "Y-1.5"
        ]);
    }
    
    [Test]
    public void OptimizesBackToBackGroupsOfXAndY()
    {
        string[] lines =
        [
            "X1.1",
            "X1.2",
            "X1.3",
            "Y1.4",
            "Y1.5",
            "Y1.6",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "X1.1",
            "X1.3",
            "Y1.4",
            "Y1.6"
        ]);
    }
    
    [Test]
    public void DoesNotOptimizeLinesThatContainBothXAndY()
    {
        string[] lines =
        [
            "X-3.0935 Y-.3356",
            "X-3.1117 Y-.3325",
            "X-3.1307 Y-.3295",
            "X-3.1501 Y-.3268",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "X-3.0935 Y-.3356",
            "X-3.1117 Y-.3325",
            "X-3.1307 Y-.3295",
            "X-3.1501 Y-.3268"
        ]);
    }
    
    [Test]
    public void OutputsLinesThatCannotBeOptimized()
    {
        string[] lines =
        [
            "Hey",
            "Hi",
            "How are you?",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "Hey",
            "Hi",
            "How are you?"
        ]);
    }
}