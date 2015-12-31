using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management;

namespace Drivelens.DetectionLibrary
{
    /// <summary>
    /// 表示一个驱动器。
    /// </summary>
    public sealed class DriveInfo
    {
        /// <summary>
        /// 用指定的 Win32_DiskDrive WMI 对象初始化 DriveInfo 对象的新实例。
        /// </summary>
        /// <param name="source"></param>
        internal DriveInfo(ManagementObject source)
        {
            this.Model = source.GetConvertedProperty("Model", Convert.ToString, null);
            this.DeviceId = source.GetConvertedProperty("DeviceId", Convert.ToString, null);
            this.InterfaceType = source.GetConvertedProperty("InterfaceType", Convert.ToString, null);
            this.Capacity = source.GetConvertedProperty("DeviceId", Convert.ToInt64, -1);
            this.SerialNumber = source.GetConvertedProperty("SerialNumber", s => Convert.ToString(s).Trim(), null);
            this.Firmware = source.GetConvertedProperty("FirmwareRevision", Convert.ToString, null);
            this.Index = source.GetConvertedProperty("Index", Convert.ToInt32, -1);

            DiskControllerInfo? info = DiskInformationUtility.GetDiskControllerInfo(source);
            this.ControllerName = info?.ControllerName;
            this.ControllerService = info?.ControllerService;
        }

        #region 属性
        /// <summary>
        /// 获取此驱动器的控制器名称。
        /// </summary>
        public string ControllerName { get; internal set; }

        /// <summary>
        /// 获取此驱动器的控制器服务名称。
        /// </summary>
        public string ControllerService { get; internal set; }

        /// <summary>
        /// 获取此驱动器的路径。
        /// </summary>
        public string DeviceId { get; internal set; }

        /// <summary>
        /// 获取此驱动器的固件版本。
        /// </summary>
        public string Firmware { get; internal set; }

        /// <summary>
        /// 获取此驱动器的型号。
        /// </summary>
        public string Model { get; internal set; }

        /// <summary>
        /// 获取此驱动器的的序列号。
        /// </summary>
        public string SerialNumber { get; internal set; }

        /// <summary>
        /// 获取此驱动器的容量。
        /// </summary>
        public long Capacity { get; internal set; }

        /// <summary>
        /// 获取此驱动器的接口类型。
        /// </summary>
        public string InterfaceType { get; internal set; }

        /// <summary>
        /// 获取此驱动器的序号。
        /// </summary>
        public int Index { get; internal set; }

        #endregion
    }

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
