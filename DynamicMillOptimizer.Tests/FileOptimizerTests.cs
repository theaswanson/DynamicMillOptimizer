using DynamicMillOptimizer.Core;
using DynamicMillOptimizer.Core.Commands;
using DynamicMillOptimizer.Core.Commands.Optimizers;
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
    
    [TestCase("G73")]
    [TestCase("G74")]
    [TestCase("G76")]
    [TestCase("G77")]
    [TestCase("G81")]
    [TestCase("G82")]
    [TestCase("G83")]
    [TestCase("G84")]
    [TestCase("G85")]
    [TestCase("G86")]
    [TestCase("G89")]
    public void DoesNotOptimizeLinesForCannedCycles(string cannedCycleCommand)
    {
        string[] lines =
        [
            $"something something {cannedCycleCommand} something",
            "X1.0",
            "X2.0",
            "X3.0",
            "G80"
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            $"something something {cannedCycleCommand} something",
            "X1.0",
            "X2.0",
            "X3.0",
            "G80"
        ]);
    }
    
    [Test]
    public void OptimizesLinesThatComeAfterCannedCycleCommand()
    {
        string[] lines =
        [
            "something something G83 something",
            "X1.0",
            "X2.0",
            "X3.0",
            "G80",
            "X1.0",
            "X2.0",
            "X3.0",
            "X4.0",
            "X5.0",
        ];

        var result = _fileOptimizer.Optimize(lines);

        result.Should().BeEquivalentTo(
        [
            "something something G83 something",
            "X1.0",
            "X2.0",
            "X3.0",
            "G80",
            "X1.0",
            "X5.0",
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