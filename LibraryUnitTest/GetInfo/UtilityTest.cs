using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiskBenchmark.Library.GetInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DiskBenchmark.Library.GetInfo.Tests
{
    [TestClass()]
    public class UtilityTest
    {
        [TestMethod()]
        public void GetPartitionsTest()
        {
            PartitionInfo[] partInfo = Utility.GetPartitions();
            Debug.Write(partInfo.Length);
            Assert.Inconclusive();
        }
    }
}