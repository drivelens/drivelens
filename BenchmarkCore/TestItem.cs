using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkCore
{
    interface TestItem
    {
        Task<int> RunTest();
    }

}
