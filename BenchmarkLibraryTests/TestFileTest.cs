using System;
using System.IO;
using DiskMagic.BenchmarkLibrary;
using DiskMagic.BenchmarkLibrary.BenchmarkProviders;
using DiskMagic.DetectionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchmarkLibraryTests
{
    [TestClass]
    public class TestFileTest
    {
        /// <summary>
        /// UtilityTest 的摘要说明
        /// </summary>
        [TestClass]
        public class UtilityTest
        {
            private TestContext testContextInstance;

            /// <summary>
            ///获取或设置测试上下文，该上下文提供
            ///有关当前测试运行及其功能的信息。
            ///</summary>
            public TestContext TestContext
            {
                get { return testContextInstance; }
                set { testContextInstance = value; }
            }

            readonly string path = @"D:\SSD测试工具临时文件夹\临时文件.tmp";

            [TestMethod]
            public void DirectoryShoudBeDeleted()
            {
                var partition = new PartitionInfo {DeviceId = "D:"};
                var type = BenchmarkType.Read;
                var size = 0x100;

                BenchmarkFile.OpenFileStream(partition, type, size, _ => { });

                bool isExit = Directory.Exists(path);
                Assert.IsFalse(isExit);
            }

            [TestMethod]
            public void ReadShoudBeRead()
            {
                var partition = new PartitionInfo {DeviceId = "D:"};
                var type = BenchmarkType.Read;
                var size = 0x100;

                BenchmarkFile.OpenFileStream(partition, type, size,
                    stream =>
                    {
                        Assert.IsTrue(stream.CanRead);
                        Assert.IsFalse(stream.CanWrite);
                    });
            }
        }
    }
}
