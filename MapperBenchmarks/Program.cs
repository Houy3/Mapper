using BenchmarkDotNet.Running;

namespace MapperBenchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<MapperTest>();
    }
}
