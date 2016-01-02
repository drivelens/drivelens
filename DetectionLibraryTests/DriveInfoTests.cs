using Microsoft.VisualStudio.TestTools.UnitTesting;
using Drivelens.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drivelens.DetectionLibrary.Objects;

namespace Drivelens.DetectionLibrary.Tests
{
    [TestClass()]
    public class DriveInfoTests
    {
        [TestMethod()]
        public void LocalDrivesTest()
        {
            var result = DriveInfo.LocalDrives;
            if(result.Count == 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RefreshPropertiesTest()
        {
            Assert.Fail();
        }
    }
}