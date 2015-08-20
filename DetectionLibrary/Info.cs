using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DiskMagic.DetectionLibrary
{
    /// <summary>
    /// 表示一个驱动器。
    /// </summary>
    public sealed class DriveInfo
    {
        /// <summary>
        /// 表示此驱动器的所有分区。
        /// </summary>
        private List<PartitionInfo> _partitions = new List<PartitionInfo>();

        #region 属性
        /// <summary>
        /// 获取此驱动器的控制器名称。
        /// </summary>
        public string ControllerName { get; internal set; }

        /// <summary>
        ///// 获取此驱动器的控制器服务名称。
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

        /// <summary>
        /// 获取此驱动器的所有分区。
        /// </summary>
        public ReadOnlyCollection<PartitionInfo> Partitions
        {
            get { return _partitions.AsReadOnly(); }
        }

        /// <summary>
        /// 向分区列表中添加项目。
        /// </summary>
        /// <param name="partition">要添加的分区。</param>
        internal void AddPartition(PartitionInfo partition)
        {
            _partitions.Add(partition);
        }
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
