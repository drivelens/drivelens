using System;
using System.IO;
using static System.Math;

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
        TResult GetTestResult(PartitionInfo partition, TType type);

        string Name { get; }
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

        public abstract string Name { get; }

        public virtual TimeSpan GetTestResult(PartitionInfo partition, BenchmarkType type)
        {
            TimeSpan result = new TimeSpan();
            BenchmarkFile.OpenFileStream(partition, type, BlockSize,
                   stream =>
                   {
                       Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);
                       result = DoBenchmarkAlgorithm(stream, work, type);
                   });
            return result;
        }

        protected abstract TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkType type);
    }

    /// <summary>
    /// 
    /// </summary>
    public class SequenceBenchmarkProvider : BenchmarkProviderBase
    {
        public override int BlockSize { get; } = 0x1000000;

        public override int BlockCount { get; } = 64;

        //TODO:本地化
        public override string Name { get; } = "连续测试";

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkType type)
        {
            var sequenceReadTimeTotal = new TimeSpan(0);
            byte[] buffer = Utility.GetData(BlockSize, type.HasFlag(BenchmarkType.Compressible));
            for (int i = 0; i < BlockCount; i++)
            {
                sequenceReadTimeTotal += Utility.GetTime(() => work(buffer, 0, buffer.Length));
            }
            return sequenceReadTimeTotal;
        }
    }

    public abstract class RandomBenchmarkProvider : BenchmarkProviderBase
    {
        protected int BlockCountValue = 0x40000;

        public override int BlockCount => this.BlockCountValue;

        public virtual double EvalutionCount => 0x200;

        Random random = new Random();
        //protected readonly int BlockSize = 4096;
        //public override TimeSpan GetTestResult(PartitionInfo partition, BenchmarkType type)
        //{
        //    TimeSpan randomBenchmarkTimeTotal = new TimeSpan(0);
        //    BenchmarkFile.OpenFileStream(partition, type, BlockSize,
        //        stream =>
        //        {
        //            Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);

        //            for (uint i = 0; i < BlockCount; i++)
        //            {
        //                var randomArray = Utility.GetData(BlockSize, type.HasFlag(BenchmarkType.Compressible));
        //                long posision = BlockSize * this.random.Next(BlockCount);
        //                randomBenchmarkTimeTotal += Utility.GetTime(() =>
        //                {
        //                    stream.Seek(posision, SeekOrigin.Begin);
        //                    work(randomArray, 0, randomArray.Length);
        //                });
        //            }
        //            //BlockCount = Math.Min((uint)(_4KWriteSpeed.SpeedInIOPerSecond * 60), BlockCount);
        //        });
        //    throw new NotImplementedException();
        //}

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkType type)
        {
            var randomBenchmarkTimeTotal = new TimeSpan(0);
            for (int i = 0; i < BlockCount; i++)
            {
                var randomArray = Utility.GetData(BlockSize, type.HasFlag(BenchmarkType.Compressible));
                long posision = BlockSize * this.random.Next(BlockCount);
                randomBenchmarkTimeTotal += Utility.GetTime(() =>
                {
                    stream.Seek(posision, SeekOrigin.Begin);
                    work(randomArray, 0, randomArray.Length);
                });

                var blockCountIn60S = (double)i / randomBenchmarkTimeTotal.Ticks * 60;
                this.BlockCountValue = (int)Min(this.BlockCountValue, Max(Max(i,EvalutionCount), blockCountIn60S));
            }
            return randomBenchmarkTimeTotal;
        }
    }

    public class Random4KBenchmarkProvider : RandomBenchmarkProvider
    {
        public override int BlockSize => 0x1000;

        //TODO:本地化
        public override string Name { get; } = "4K随机测试";
    }

    public class Random512KBenchmarkProvider : RandomBenchmarkProvider
    {
        public override int BlockSize => 0x80000;

        //TODO:本地化
        public override string Name { get; } = "512K随机测试";
    }
}
