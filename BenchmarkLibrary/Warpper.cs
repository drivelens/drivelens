using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.BenchmarkLibrary
{
    public static class BenchmarkTestProviders
    {
        public static readonly SequenceBenchmarkProvider SequenceBenchmarkProvider = new SequenceBenchmarkProvider();
        public static readonly Random4KBenchmarkProvider Random4KBenchmarkProvider = new Random4KBenchmarkProvider();
        public static readonly Random512KBenchmarkProvider Random512KBenchmarkProvider = new Random512KBenchmarkProvider();
        public static readonly Random4K64ThreadRandomBenchmarkProvider Random4K64ThreadRandomBenchmarkProvider = new Random4K64ThreadRandomBenchmarkProvider();
    }

    //public static class Benchmarker<T>
    //{
    //    public static IBenchmarkProvider<T, BenchmarkType> Benchmark { get; set; }

    //    public static 
    //}
}
