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
        public List<IBenchmarkProvider> BenchmarkProviders => new List<IBenchmarkProvider>
        {
            BenchmarkTestProviders.SequenceBenchmarkProvider,
            BenchmarkTestProviders.Random4KBenchmarkProvider,
            BenchmarkTestProviders.Random512KBenchmarkProvider,
            BenchmarkTestProviders.Random4K64ThreadRandomBenchmarkProvider,
        };
    }
}
