using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary
{
    /// <summary>
    /// 表示一个分区。
    /// </summary>
    public sealed class PartitionInfo
    {
        ///// <summary>
        ///// 表示此分区所属的驱动器。
        ///// </summary>
        //DriveInfo drive;

        public PartitionInfo(ManagementObject source)
        {
            // 基础信息
            this.Capacity = source.GetConvertedProperty("Size", Convert.ToInt64, -1);            // 分区大小
            this.FreeSpace = source.GetConvertedProperty("FreeSpace", Convert.ToInt64, -1);      // 剩余空间
            this.PartitionType = source.GetConvertedProperty("DriveType", Convert.ToInt32, -1);    // 磁盘类型
            this.FileSystem = source.GetConvertedProperty("FileSystem", Convert.ToString, null);         // 文件系统
            this.DeviceId = source.GetConvertedProperty("DeviceId", Convert.ToString, null);             // 盘符
            this.VolumeName = source.GetConvertedProperty("VolumeName", Convert.ToString, null);         // 卷标
            this.SerialNumber = source.GetConvertedProperty("VolumeSerialNumber", s => Convert.ToString(s).Trim(), null);  // 分区序列号，头尾去除空格

            // 卷信息
            this.BlockSize = DiskInformationUtility.GetPartitionBlockSize(this.DeviceId); // 分区的分配单元大小

            DiskPartitionInfo? indexAndStartingOffset = DiskInformationUtility.GetDiskPartitionIndexAndStartingOffset(this.DeviceId);
            this.Index = indexAndStartingOffset?.Index;                        // 分区的索引
            this.StartingOffset = indexAndStartingOffset?.StartingOffset;      // 分区起始偏移
        }

        /// <summary>
        /// 获取此分区的区块大小。
        /// </summary>
        public long? BlockSize { get; internal set; }

        /// <summary>
        /// 获取此分区所分配的盘符。
        /// </summary>
#if DEBUG
        public string DeviceId { get; set; }
#else
        public string DeviceId { get; internal set; }
#endif

        /// <summary>
        /// 获取此分区的起始偏移。
        /// </summary>
        public long? StartingOffset { get; internal set; }

        /// <summary>
        /// 获取此分区的容量。
        /// </summary>
        public long Capacity { get; internal set; }

        /// <summary>
        /// 获取此分区的序号。
        /// </summary>
        public int? Index { get; internal set; }

        /// <summary>
        /// 获取此分区的卷标。
        /// </summary>
        public string VolumeName { get; internal set; }

        /// <summary>
        /// 获取此分区的序列号。
        /// </summary>
        public string SerialNumber { get; internal set; }

        /// <summary>
        /// 获取此分区的类型。
        /// </summary>
        public int PartitionType { get; internal set; }

        /// <summary>
        /// 获取此分区的空闲空间。
        /// </summary>
        public long FreeSpace { get; internal set; }

        /// <summary>
        /// 获取此分区的文件系统。
        /// </summary>
        public string FileSystem { get; internal set; }

        /// <summary>
        /// 获取此分区所属的磁盘。
        /// </summary>
        public DriveInfo Drive
        {
            get;
            internal set;

        }
    }
}
