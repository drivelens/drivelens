using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management;
using Drivelens.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Drivelens.DetectionLibrary.Tests
{
    [TestClass()]
    public class UtilityTest
    {
        [TestMethod()]
        public void GetVolumeObjectFromDeviceIdTest()
        {
            ManagementObject mo = WmiUtility.GetVolumeObjectByDeviceId("C:");
            Assert.AreEqual("C:\\", (string)mo["Name"]);
        }

        [TestMethod()]
        public void GetDiskPartitionObjectByDeviceIdTest()
        {
            ManagementObject mo = WmiUtility.GetDiskPartitionObjectByLogicalDiskDeviceId("C:");
            Assert.IsTrue((ulong)mo["Size"] > 0);
        }

        [TestMethod()]
        public void GetDiskDriveObjectByDiskPartitionTest()
        {
            ManagementObject mo = WmiUtility.GetDiskDriveObjectByPartitionId((string)WmiUtility.GetFirstObjectOrNull("SELECT * FROM Win32_DiskPartition")["DeviceId"]);
            Assert.IsTrue((ulong)mo["Size"] > 0);
        }
    }
}