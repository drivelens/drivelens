using Microsoft.VisualStudio.TestTools.UnitTesting;
using Drivelens.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary.Tests
{
    [TestClass()]
    public class DiskObjectsTests
    {
        [TestMethod()]
        public void InitalizeListTest()
        {
            DiskObjects.InitalizeList();
            Assert.IsTrue(DiskObjects.AllDrives.Count != 0);
            Assert.IsTrue(DiskObjects.AllPartitions.Count != 0);
        }
    }
}