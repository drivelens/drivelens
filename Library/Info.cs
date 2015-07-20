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
    public class DiskInfo
    {
        private PartitionInfo[] _partitions;

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
        public string DeviceID { get; internal set; }

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
        public string Type { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public PartitionInfo[] Partitions
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
    public class PartitionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong BlockSize { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceID { get; internal set; }

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
        public uint Type { get; internal set; }

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
        public DiskInfo Drive { get; internal set; }
    }
}
