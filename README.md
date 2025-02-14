# DynamicMillOptimizer

This program optimizes dynamic milling files by reducing the number of commands used to move the tool in a straight line.

For example, given the following code:

```text
X1.0
X2.0
X3.0
X4.0
X5.0
```

The program will optimize it into:

```text
X1.0
X5.0
```

## How to Compile

1. Install the [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).
2. Clone the repository.
3. Run `dotnet publish DynamicMillOptimizer.sln`. This will create an executable under `DynamicMillOptimizer.Console\bin\Release\net9.0\{platform}\publish`.