using DiskMagic.DetectionLibrary;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssemblyUtility = DiskMagic.BenchmarkLibrary.Utility;

namespace DiskMagic.BenchmarkLibrary.BenchmarkProviders
{
    /// <summary>
    /// 表示连续测试。
    /// </summary>
    public class SequenceBenchmarkProvider : BenchmarkProviderBase
    {
        internal SequenceBenchmarkProvider() { }

        public override int BlockSize => 0x1000000;

        public override int BlockCount => 0X40;

        //TODO:本地化
        public override string Name => "连续测试";

        protected override TimeSpan DoBenchmarkAlgorithm(FileStream stream, Action<byte[], int, int> work, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            var sequenceReadTimeTotal = new TimeSpan(0);
            byte[] buffer = AssemblyUtility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
            for (int i = 0; i < BlockCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();   //可以取消
                sequenceReadTimeTotal += AssemblyUtility.GetTime(() => work(buffer, 0, buffer.Length));
            }
            return sequenceReadTimeTotal;
        }
    }
}