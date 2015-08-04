using DiskMagic.DetectionLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AssemblyUtility = DiskMagic.BenchmarkLibrary.Utility;

namespace DiskMagic.BenchmarkLibrary.BenchmarkProviders
{
    /// <summary>
    /// 表示4K64线程测试。
    /// </summary>
    public class Random4K64ThreadBenchmarkProvider : BenchmarkProviderBase
    {
        internal Random4K64ThreadBenchmarkProvider() { }

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

                var randomArray = AssemblyUtility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
                long posision = BlockSize * random.Next(BlockCount);
                timeTotal += AssemblyUtility.GetTime(() =>
                {
                    stream.Seek(posision, SeekOrigin.Begin);
                    work(randomArray, 0, randomArray.Length);
                });
            }
            return timeTotal;
        }
    }
}
