using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DiskMagic.BenchmarkLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchmarkLibraryTests
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
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }

        #region 附加测试特性

        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void ForEachMethodIsEquivalentToKeyword()
        {
            var source = new[] {1, 2, 3, 6, 8, 3, 4, 6, 1, 2};
            var useMethod = "";
            var useKeyword = "";

            source.ForEach(item => useMethod += item);
            foreach (var item in source)
            {
                useKeyword += item;
            }

            Assert.AreEqual(useKeyword, useMethod);
        }

        readonly Action testAction = () =>
        {
            Enumerable.Range(0, 10000)
                .ForEach(n => { });
        };
        
        [TestMethod]
        public void TimeIsSimilar()
        {
            var result1 = Utility.GetTime(this.testAction);
            var result2 = Utility.GetTime(this.testAction);
            var r = result1 - result2;

            Assert.IsTrue(r < new TimeSpan(100000));
        }

        [TestMethod]
        public void SizeIsRight()
        {
            var sizes = new[] {256, 512, 1024};
            foreach (var size in sizes)
            {

                var data = Utility.GetData(size, true);

                Assert.AreEqual(size, data.Length);
            }
        }

        [TestMethod]
        public void UncompressibleShoudDifferent()
        {
            var size = 256;
            var datas = new byte[5][];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = Utility.GetData(size, false);
            }

            for (int i = 0; i < datas.Length - 1; i++)
            {
                for (int j = i + 1; j < datas.Length; j++)
                {
                    Assert.AreNotEqual(datas[i], datas[j]);
                }
            }
        }
    }
}
