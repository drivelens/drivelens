using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskBenchmark.Library
{
    ///// <summary>
    ///// 表示一个单独的测试
    ///// </summary>
    ///// <typeparam name="TResult">测试结果类型。</typeparam>
    //public interface ITestItem<TResult>
    //{
    //    Task<TResult> RunTest();
    //}

    /// <summary>
    /// 表示一类测试。
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public interface ITestProvider<TResult, TType>
    {
        TResult GetTest(PartitionInfo partition, TType type);
    }

    /// <summary>
    /// 测试类型：读/写。
    /// </summary>
    [Flags]
    public enum TestType
    {
        None = 0x0,
        Read = 0x01,
        Write = 0x02,
        Compressible = 0x04,
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class TestClass : ITestItem<TimeSpan>
    //{
    //    Func<Task<TimeSpan>> testWork;

    //    public TestClass(Func<Task<TimeSpan>> test)
    //    {
    //        testWork = test;
    //    }

    //    public Task<TimeSpan> RunTest()
    //    {
    //        return testWork();
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public abstract class TestBase : ITestProvider<TimeSpan, TestType>
    {
        public abstract TimeSpan GetTest(PartitionInfo partition, TestType type);
    }

    /// <summary>
    /// 
    /// </summary>
    public class SequenceTestProvider : TestBase
    {
        private const int SequenceBlockSize = 0x1000000; // 16MB
        private const int SequenceBlockCount = 64;


        public override TimeSpan GetTest(PartitionInfo partition, TestType type)
        {
            TimeSpan sequenceReadTimeTotal = new TimeSpan(0);
            TestFile.OpenFileStream(partition, type, SequenceBlockSize,
                stream =>
                {
                    byte[] buffer = Utility.GetData(SequenceBlockSize, type.HasFlag(TestType.Compressible));
                    for (int i = 0; i < SequenceBlockCount; i++)
                    {
                        sequenceReadTimeTotal += Utility.GetTime(() => stream.Read(buffer, 0, buffer.Length));
                    }
                });
            return sequenceReadTimeTotal;
        }
    }
}
