using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskBenchmark.Library
{
    /// <summary>
    /// 硬盘信息
    /// </summary>
    public sealed class DiskInfo
    {
        private List<PartitionInfo> _partitions;

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public string ControllerName { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string ControllerService { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Firmware { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Model { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string SerialNumber { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong Capacity { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string DiskType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PartitionInfo> Partitions
        {
            get { return _partitions; }
            internal set
            {
                _partitions = value;
                _partitions.ForEach(partition => partition.Drive = this);
            }
        }
        #endregion
    }

    /// <summary>
    /// 分区信息
    /// </summary>
    public sealed class PartitionInfo
    {
        DiskInfo drive;

        /// <summary>
        /// 
        /// </summary>
        public ulong BlockSize { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong StartingOffset { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong Capacity { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string VolumeName { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string SerialNumber { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public uint PartionType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong FreeSpace { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileSystem { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public DiskInfo Drive
        {
            get { return drive; }
            internal set
            {
                drive = value;
                drive.Partitions.Add(this);
            }
        }
    }
}
