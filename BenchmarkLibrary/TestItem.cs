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
    public interface IBenchmarkProvider<out TResult, in TType>
    {
        TResult GetTest(PartitionInfo partition, TType type);
    }

    /// <summary>
    /// 测试类型：读/写。
    /// </summary>
    [Flags]
    public enum BenchmarkType
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
    public class SequenceBenchmarkProvider : IBenchmarkProvider<TimeSpan, BenchmarkType>
    {
        readonly int blockSize = 0x1000000; // 16MB
        readonly int blockCount = 64;

        protected virtual int BlockSize
        {
            get { return this.blockSize; }
        }

        protected virtual int BlockCount
        {
            get { return this.blockCount; }
        }


        public virtual TimeSpan GetTest(PartitionInfo partition, BenchmarkType type)
        {
            var sequenceReadTimeTotal = new TimeSpan(0);
            TestFile.OpenFileStream(partition, type, BlockSize,
                stream =>
                {
                    Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);

                    byte[] buffer = Utility.GetData(BlockSize, type.HasFlag(BenchmarkType.Compressible));
                    for (int i = 0; i < BlockCount; i++)
                    {
                        sequenceReadTimeTotal += Utility.GetTime(() => work(buffer, 0, buffer.Length));
                    }
                });
            return sequenceReadTimeTotal;
        }
    }

    public abstract class RandomBenchmarkProvider : IBenchmarkProvider<TimeSpan, BenchmarkType>
    {
        //protected readonly int BlockSize = 4096;
        protected abstract int BlockSize { get; }

        protected abstract int BlockCount { get; }

        public TimeSpan GetTest(PartitionInfo partition, BenchmarkType type)
        {
            throw new NotImplementedException();
        }
    }
}
