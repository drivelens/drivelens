using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiskMagic.BenchmarkLibrary;

namespace DiskMagic.UI
{
    public class BenchmarkViewModel
    {
        public List<IBenchmarkProvider<object>> BenchmarkProviders => new List<IBenchmarkProvider<object>>
        {
            (IBenchmarkProvider<object>)BenchmarkTestProviders.SequenceBenchmarkProvider,
            (IBenchmarkProvider<object>)BenchmarkTestProviders.Random4KBenchmarkProvider,
            (IBenchmarkProvider<object>)BenchmarkTestProviders.Random512KBenchmarkProvider,
            (IBenchmarkProvider<object>)BenchmarkTestProviders.Random4K64ThreadRandomBenchmarkProvider,
        };
    }
}
