using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management;
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
        public void GetVolumeObjectFromDeviceIdTest()
        {
            ManagementObject mo = Utility.GetVolumeObjectByDeviceId("C:");
            Assert.AreEqual("C:\\", (string)mo["Name"]);
        }

        [TestMethod()]
        public void GetDiskPartitionObjectByDeviceIdTest()
        {
            ManagementObject mo = Utility.GetDiskPartitionObjectByDeviceId("C:");
            Assert.IsTrue((ulong)mo["Size"] > 0);
        }

        [TestMethod()]
        public void GetDiskDriveObjectByDiskPartitionIdTest()
        {
            ManagementObject mo = Utility.GetDiskDriveObjectByDiskPartitionId("Disk #0, Partition #2");
            Assert.IsTrue((ulong)mo["Size"] > 0);
        }
    }
}