using DiskMagic.DetectionLibrary;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;

namespace DiskMagic.BenchmarkLibrary
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
    public interface IBenchmarkProvider
    {
        /// <summary>
        /// 返回测试结果。
        /// </summary>
        /// <param name="partition">测试分区</param>
        /// <param name="arg">测试所需参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IOSpeed GetTestResult(PartitionInfo partition, BenchmarkType arg, BenchmarkFlags flags, CancellationToken cancellationToken);

        /// <summary>
        /// 获取测试的名称。
        /// </summary>
        string Name { get; }
    }

    // TODO:需要更清楚的摘要。
    /// <summary>
    /// 测试类型
    /// </summary>
    public enum BenchmarkType
    {
        /// <summary>
        /// 读测试。
        /// </summary>
        Read = 0x01,

        /// <summary>
        /// 写测试。
        /// </summary>
        Write = 0x02,
    }

    [Flags]
    public enum BenchmarkFlags
    {
        /// <summary>
        /// 默认。
        /// </summary>
        None = 0x0,

        /// <summary>
        /// 可压缩。
        /// </summary>
        Compressible = 0x04,
    }

    /// <summary>
    /// 用作分块读写测试的基类。
    /// </summary>
    public abstract class BenchmarkProviderBase : IBenchmarkProvider
    {
        /// <summary>
        /// 获取每块大小。
        /// </summary>
        public abstract int BlockSize { get; }

        /// <summary>
        /// 获取总块数。
        /// </summary>
        public abstract int BlockCount { get; }

        public abstract string Name { get; }

        public virtual IOSpeed GetTestResult(PartitionInfo partition, BenchmarkType type, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            TimeSpan result = new TimeSpan();
            BenchmarkFile.OpenFileStream(partition, type, BlockSize,
                stream =>
                {
                    Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);
                    result = DoBenchmarkAlgorithm(stream, work, flags, cancellationToken);
                });
            return new IOSpeed(time: result, ioCount: BlockCount, bytes: BlockCount * BlockSize);
        }

        /// <summary>
        /// 测试核心算法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="work"></param>
        /// <param name="flags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkFlags flags, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 表示连续测试。
    /// </summary>
    public class SequenceBenchmarkProvider : BenchmarkProviderBase
    {
        internal SequenceBenchmarkProvider() { }

        public override int BlockSize { get; } = 0x1000000;

        public override int BlockCount { get; } = 0X40;

        //TODO:本地化
        public override string Name { get; } = "连续测试";

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            var sequenceReadTimeTotal = new TimeSpan(0);
            byte[] buffer = Utility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
            for (int i = 0; i < BlockCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();   //可以取消
                sequenceReadTimeTotal += Utility.GetTime(() => work(buffer, 0, buffer.Length));
            }
            return sequenceReadTimeTotal;
        }
    }

    /// <summary>
    /// 用作随机测试的基类。
    /// </summary>
    public abstract class RandomBenchmarkProviderBase : BenchmarkProviderBase
    {
        public override int BlockCount => this.BlockCountValue;

        /// <summary>
        /// 用于内部可以调整的BlockCount
        /// </summary>
        protected abstract int BlockCountValue { get; set; }

        public abstract double EvalutionCount { get; }

        protected Random RandomGen = new Random();

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            var randomBenchmarkTimeTotal = new TimeSpan(0);
            for (int i = 0; i < BlockCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();   //可以取消

                var randomArray = Utility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
                long posision = BlockSize * this.RandomGen.Next(BlockCount);
                randomBenchmarkTimeTotal += Utility.GetTime(() =>
                {
                    stream.Seek(posision, SeekOrigin.Begin);
                    work(randomArray, 0, randomArray.Length);
                });

                var blockCountIn60S = (double) i / randomBenchmarkTimeTotal.Ticks * 60;
                this.BlockCountValue = (int) Min(this.BlockCountValue, Max(Max(i, EvalutionCount), blockCountIn60S));
            }
            return randomBenchmarkTimeTotal;
        }
    }

    /// <summary>
    /// 表示4K随机测试。
    /// </summary>
    public class Random4KBenchmarkProvider : RandomBenchmarkProviderBase
    {
        internal Random4KBenchmarkProvider() { }

        public override int BlockSize => 0x1000;

        protected override int BlockCountValue { get; set; } = 0x40000;

        public override double EvalutionCount => 0x200;

        //TODO:本地化
        public override string Name { get; } = "4K随机测试";
    }

    /// <summary>
    /// 表示512K随机测试。
    /// </summary>
    public class Random512KBenchmarkProvider : RandomBenchmarkProviderBase
    {
        internal Random512KBenchmarkProvider() { }

        public override int BlockSize => 0x80000;

        protected override int BlockCountValue { get; set; } = 0x800;

        public override double EvalutionCount => 0x04;

        //TODO:本地化
        public override string Name { get; } = "512K随机测试";
    }

    /// <summary>
    /// 表示4K64线程测试。
    /// </summary>
    public class Random4K64ThreadRandomBenchmarkProvider : BenchmarkProviderBase
    {
        internal Random4K64ThreadRandomBenchmarkProvider() { }

        public override int BlockSize { get; } = 0x1000;

        public override int BlockCount { get; } = 0x40;

        public override string Name { get; } = "4K64线程";

        readonly int outstandingThreadsCount = 0x40;

        public override IOSpeed GetTestResult(PartitionInfo partition, BenchmarkType type, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            var randomBenchmarkTimes = new TimeSpan[this.outstandingThreadsCount];
            BenchmarkFile.OpenFileHandle(partition, type,
                handle =>
                {
                    Parallel.For(0, this.outstandingThreadsCount, i =>
                    {
                        using (FileStream stream = new FileStream(handle, FileAccess.Read, BlockSize))
                        {
                            Action<byte[], int, int> work = Utility.GetReadOrWriteAction(type, stream);
                            randomBenchmarkTimes[i] = DoBenchmarkAlgorithm(stream, work, flags, cancellationToken);
                        }
                    });
                });
            var totalTimes = randomBenchmarkTimes.Aggregate((a, b) => a + b);
            return new IOSpeed(time: totalTimes, ioCount: BlockCount, bytes: BlockCount * BlockSize);
        }

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            var random = new Random();
            TimeSpan timeTotal = default(TimeSpan);
            for (int j = 0; j < BlockCount / this.outstandingThreadsCount; j++)
            {
                cancellationToken.ThrowIfCancellationRequested();   //可以取消

                var randomArray = Utility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
                long posision = BlockSize * random.Next(BlockCount);
                timeTotal += Utility.GetTime(() =>
                {
                    stream.Seek(posision, SeekOrigin.Begin);
                    work(randomArray, 0, randomArray.Length);
                });
            }
            return timeTotal;
        }
    }
}
