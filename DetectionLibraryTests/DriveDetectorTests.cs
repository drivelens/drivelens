using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiskMagic.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary.Tests
{
    [TestClass()]
    public class DriveDetectorTests
    {
        [TestMethod()]
        public void GetPartitionsTest()
        {
            PartitionInfo[] infos = DriveDetector.GetPartitions();
            if(infos.Length > 0)
            {
                return;
            }
            Assert.Fail();
        }
    }
}