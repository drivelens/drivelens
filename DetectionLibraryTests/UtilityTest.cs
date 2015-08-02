using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiskMagic.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DiskMagic.DetectionLibrary.Tests
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