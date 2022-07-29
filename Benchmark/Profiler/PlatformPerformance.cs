namespace Benchmark.Profiler
{
    public abstract class APlatformPerformance
    {
        public static APlatformPerformance Instance { get; set; }

        public abstract string GetMemoryInfo();
    }
}
