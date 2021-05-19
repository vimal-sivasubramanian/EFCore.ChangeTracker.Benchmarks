using BenchmarkDotNet.Running;

namespace EFCore.ChangeTracker.Benchmarks
{
    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<EFCoreTest>();
    }
}
