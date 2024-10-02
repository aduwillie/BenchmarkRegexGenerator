using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.RegularExpressions;

// Reference
// https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7/#:~:text=Now%20with%20.NET%207,%20we%E2%80%99ve%20again%20heavily%20invested%20in%20improving

BenchmarkRunner.Run<Benchmark>();

[MemoryDiagnoser]
[SimpleJob(iterationCount: 10)]
public class Benchmark
{
    private const string sampleEmail = "aduwillie@gmail.com";

    [Benchmark]
    public bool IsMatchWithCompiled() => RegexTester.IsMatchWithCompiled(sampleEmail);

    [Benchmark(Baseline = true)]
    public bool IsMatchWithoutCompiled() => RegexTester.IsMatchWithoutCompiled(sampleEmail);
}

static partial class RegexTester
{
    private const string RegexPattern = "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$";

    [GeneratedRegex(RegexPattern, RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    public static bool IsMatchWithCompiled(string input)
    {
        return EmailRegex().IsMatch(input);
    }

    public static bool IsMatchWithoutCompiled(string input)
    {
        return new Regex(RegexPattern).IsMatch(input);
    }
}
