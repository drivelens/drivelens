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
            ManagementObject mo = Utility.GetVolumeObjectFromDeviceId("C:");
            UInt64 s = (UInt64)mo["BlockSize"];
        }
    }
}