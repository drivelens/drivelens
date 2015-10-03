using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.BenchmarkLibrary
{
    /// <summary>
    /// 表示读写速度。
    /// </summary>
    public struct IOSpeed
    {
        /// <summary>
        /// 初始化IOSpeed结构的新实例。
        /// </summary>
        /// <param name="time">所用时间</param>
        /// <param name="ioCount">所用IO操作次数</param>
        /// <param name="bytes">读写字节数</param>
        public IOSpeed(TimeSpan time, int ioCount, long bytes)
        {
            Time = time;
            IOCount = ioCount;
            Bytes = bytes;
        }

        /// <summary>
        /// 所用时间。
        /// </summary>
        public TimeSpan Time { get; private set; }

        /// <summary>
        /// 所用IO操作次数。
        /// </summary>
        public int IOCount { get; private set; }

        /// <summary>
        /// 读写字节数。
        /// </summary>
        public long Bytes { get; private set; }

        /// <summary>
        /// 读写兆字节数。
        /// </summary>
        public long Megabytes => Bytes / 0x100000;

        /// <summary>
        /// 每秒读写兆字节数。
        /// </summary>
        public double MegabytePerSecond => Megabytes / Time.TotalSeconds;

        /// <summary>
        /// 每秒IO操作数。
        /// </summary>
        public double IOPerSecond => IOCount / Time.TotalSeconds;
    }
}
