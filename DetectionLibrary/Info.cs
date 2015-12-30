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
        /// 从 Win32_DiskDrive WMI 对象创建 DriveInfo 对象。
        /// </summary>
        /// <param name="source"></param>
        internal DriveInfo(ManagementObject source)
        {
            this.Model = source["Model"].ToString();
            this.DeviceId = (string)source["DeviceID"];
            this.InterfaceType = (string)source["InterfaceType"];
            this.Capacity = source["Size"] != null ? (long)(ulong)source["Size"] : 0;
            this.SerialNumber = ((string)source["SerialNumber"]).Trim();
            this.Firmware = (string)source["FirmwareRevision"];
            this.Index = (int)(uint)source["Index"];

            DiskControllerInfo? info = DiskInformationUtility.GetDiskControllerInfo(source);
            if (info.HasValue)
            {
                this.ControllerName = info.Value.ControllerName;
                this.ControllerService = info.Value.ControllerService;
            }
            else
            {
                this.ControllerName = this.ControllerService = "";
            }
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
        /// <summary>
        /// 表示此分区所属的驱动器。
        /// </summary>
        DriveInfo drive;



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
