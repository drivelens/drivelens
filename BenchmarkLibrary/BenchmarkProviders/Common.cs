using Drivelens.DetectionLibrary;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Drivelens.BenchmarkLibrary.BenchmarkProviders.Utility;

namespace Drivelens.BenchmarkLibrary
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
        /// <param name="arg">测试类型</param>
        /// <param name="flags">测试所需参数</param>
        /// <param name="cancellationToken">用以取消工作的取消标记</param>
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

        /// <summary>
        /// 根据核心测试算法返回的时间计算结果。
        /// </summary>
        /// <param name="partition">测试分区</param>
        /// <param name="arg">测试类型</param>
        /// <param name="flags">测试所需参数</param>
        /// <param name="cancellationToken">用以取消工作的取消标记</param>
        /// <returns></returns>
        public virtual IOSpeed GetTestResult(PartitionInfo partition, BenchmarkType type, BenchmarkFlags flags, CancellationToken cancellationToken)
        {
            TimeSpan result = new TimeSpan();
            BenchmarkFile.OpenFileStream(partition, type, BlockSize,
                stream =>
                {
                    Action<byte[], int, int> work = GetReadOrWriteAction(type, stream);
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
}
