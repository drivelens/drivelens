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
    public sealed class LogicalDiskInfo : WmiDeviceInfoObjectBase
    {
        /// <summary>
        /// 用指定的 WMI 对象（Win23_LogicalDisk）初始化 PartitionInfo 类的新实例。
        /// </summary>
        /// <param name="source">用于初始化的 WMI 对象（Win23_LogicalDisk）。</param>
        internal LogicalDiskInfo(ManagementObject source) : base(source)
        {
            RefreshPropertiesFromWmiObject(source);
        }


        /// <summary>
        /// 刷新本实例所包含的分区信息。
        /// </summary>
        public override void RefreshProperties()
        {
            RefreshPropertiesFromWmiObject(WmiUtility.GetLogicalDiskObjectById(this.DeviceId));
        }

        /// <summary>
        /// 刷新分区信息。
        /// </summary>
        /// <param name="source">用于获取信息的 WMI 对象（Win23_LogicalDisk）。</param>
        protected override void RefreshPropertiesFromWmiObject(ManagementObject source)
        {
            base.RefreshPropertiesFromWmiObject(source);
            // 基础信息
            this.Capacity = source.GetConvertedProperty("Size", Convert.ToInt64, -1);                   // 分区大小
            this.FreeSpace = source.GetConvertedProperty("FreeSpace", Convert.ToInt64, -1);      // 剩余空间
            this.PartitionType = source.GetConvertedProperty("DriveType", Convert.ToInt32, -1);    // 磁盘类型
            this.FileSystem = source.GetConvertedProperty("FileSystem", Convert.ToString, null);         // 文件系统
            this.DeviceId = source.GetConvertedProperty("DeviceId", Convert.ToString, null);             // 盘符
            this.VolumeName = source.GetConvertedProperty("VolumeName", Convert.ToString, null);         // 卷标
            this.SerialNumber = source.GetConvertedProperty("VolumeSerialNumber", s => Convert.ToString(s).Trim(), null);  // 分区序列号，头尾去除空格

            // 卷信息
            this.BlockSize = DiskInformationUtility.GetPartitionBlockSize(this.DeviceId); // 分区的分配单元大小

            ManagementObject partition = WmiUtility.GetDiskPartitionObjectByLogicalDiskDeviceId(this.DeviceId);
            DiskPartitionInfo? indexAndStartingOffset =
                DiskInformationUtility.GetDiskPartitionIndexAndStartingOffset(partition);
            this.Index = indexAndStartingOffset?.Index;                        // 分区的索引
            this.StartingOffset = indexAndStartingOffset?.StartingOffset;      // 分区起始偏移
        }

        #region 属性
        /// <summary>
        /// 获取此分区的区块大小。
        /// </summary>
        public long? BlockSize { get; private set; }

        /// <summary>
        /// 获取此分区所分配的盘符。
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// 获取此分区的起始偏移。
        /// </summary>
        public long? StartingOffset { get; private set; }

        /// <summary>
        /// 获取此分区的容量。
        /// </summary>
        public long Capacity { get; private set; }

        /// <summary>
        /// 获取此分区的序号。
        /// </summary>
        public int? Index { get; private set; }

        /// <summary>
        /// 获取此分区的卷标。
        /// </summary>
        public string VolumeName { get; private set; }

        /// <summary>
        /// 获取此分区的序列号。
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// 获取此分区的类型。
        /// </summary>
        public int PartitionType { get; private set; }

        /// <summary>
        /// 获取此分区的空闲空间。
        /// </summary>
        public long FreeSpace { get; private set; }

        /// <summary>
        /// 获取此分区的文件系统。
        /// </summary>
        public string FileSystem { get; private set; }
        #endregion

        /// <summary>
        /// 获取此分区所属的磁盘。
        /// </summary>
        public Lazy<DriveInfo> Drive
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取标识符。
        /// </summary>
        public string Identifier
        {
            get
            {
                return DeviceId;
            }
        }
    }
}
