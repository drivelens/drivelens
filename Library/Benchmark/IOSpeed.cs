using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace DiskBenchmark.Library
{
    public struct IOSpeed
    {
        public double MegabytePerSecond { get; }
        
        public IOSpeed(double mbPerS)
        {
            MegabytePerSecond = mbPerS;
        }
    }
}
