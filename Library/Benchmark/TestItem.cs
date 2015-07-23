using System;
using System.Collections.Generic;
using System.IO;
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

    public abstract class BenchmarkProviderBase : IBenchmarkProvider<TimeSpan, BenchmarkType>
    {
        public abstract int BlockSize { get; }

        public abstract int BlockCount { get; }

        public abstract TimeSpan GetTest(PartitionInfo partition, BenchmarkType type);
    }

    /// <summary>
    /// 
    /// </summary>
    public class SequenceBenchmarkProvider : BenchmarkProviderBase
    {
        readonly int blockSize = 0x1000000; // 16MB
        readonly int blockCount = 64;

        public override int BlockSize
        {
            get { return this.blockSize; }
        }

        public override int BlockCount
        {
            get { return this.blockCount; }
        }

        public override TimeSpan GetTest(PartitionInfo partition, BenchmarkType type)
        {
            var sequenceReadTimeTotal = new TimeSpan(0);
            BenchmarkFile.OpenFileStream(partition, type, BlockSize,
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

    public abstract class RandomBenchmarkProvider : BenchmarkProviderBase
    {
        Random random = new Random();
        //protected readonly int BlockSize = 4096;
        public override TimeSpan GetTest(PartitionInfo partition, BenchmarkType type)
        {
            BenchmarkFile.OpenFileStream(partition, type, BlockSize, stream =>
            {
                Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);

                TimeSpan randomBenchmarkTimeTotal = new TimeSpan(0);
                for (uint i = 0; i < BlockCount; i++)
                {
                    var randomArray = Utility.GetData(BlockSize, type.HasFlag(BenchmarkType.Compressible));
                    long posision = BlockSize * this.random.Next(BlockCount);
                    randomBenchmarkTimeTotal += Utility.GetTime(() =>
                    {
                        stream.Seek(posision, SeekOrigin.Begin);
                        work(randomArray, 0, randomArray.Length);
                    });
                }
                //BlockCount = Math.Min((uint)(_4KWriteSpeed.SpeedInIOPerSecond * 60), BlockCount);
            });
            throw new NotImplementedException();
        }
    }
}
