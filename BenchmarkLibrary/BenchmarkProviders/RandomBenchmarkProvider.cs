using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using static System.Math;
using AssemblyUtility = DiskMagic.BenchmarkLibrary.Utility;


namespace DiskMagic.BenchmarkLibrary.BenchmarkProviders
{

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

                var randomArray = AssemblyUtility.GetData(BlockSize, flags.HasFlag(BenchmarkFlags.Compressible));
                long posision = BlockSize * this.RandomGen.Next(BlockCount);
                randomBenchmarkTimeTotal += AssemblyUtility.GetTime(() =>
                {
                    stream.Seek(posision, SeekOrigin.Begin);
                    work(randomArray, 0, randomArray.Length);
                });

                var blockCountIn60S = (double)i / randomBenchmarkTimeTotal.Ticks * 60;
                this.BlockCountValue = (int)Min(this.BlockCountValue, Max(Max(i, EvalutionCount), blockCountIn60S));
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
}
