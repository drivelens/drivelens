using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DiskMagic.DetectionLibrary
{
    public static class DiskObjects
    {
        /// <summary>
        /// 获取一个值，该值指示分区信息是否已加载。
        /// </summary>
        internal static bool PartitionInitalized { get; private set; }

        /// <summary>
        /// 获取一个值，该值指示驱动器信息是否已加载。
        /// </summary>
        internal static bool DriveInitalized { get; private set; }

        private static ReadOnlyCollection<PartitionInfo> _allPartitions;

        private static ReadOnlyCollection<DriveInfo> _allDrives;

        // 参见 DrivesInternal
        internal static ReadOnlyCollection<PartitionInfo> PartitionsInternal
        {
            get
            {
                return _allPartitions;
            }
        }

        /// <summary>
        /// 获取计算机上所有磁盘。
        /// </summary>
        public static ReadOnlyCollection<PartitionInfo> AllPartitions
        {
            get
            {
                if (!Initalized)
                {
                    //throw new InvalidOperationException();
                    InitalizeList();
                }
                return _allPartitions;
            }
        }

        /// <summary>
        /// 在没有正式加载之前程序内部使用此属性访问。此时 Drives 的 Partitions 列表还没有加载完全。
        /// </summary>
        internal static ReadOnlyCollection<DriveInfo> DrivesInternal
        {
            get
            {
                return _allDrives;
            }
        }

        /// <summary>
        /// 获取此计算机上的所有驱动器。
        /// </summary>
        public static ReadOnlyCollection<DriveInfo> AllDrives
        {
            get
            {
                if (!Initalized)
                {
                    throw new InvalidOperationException();
                }
                return _allDrives;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示列表是否已被初始化。
        /// </summary>
        public static bool Initalized { get; private set; }

        /// <summary>
        /// 初始化列表。
        /// </summary>
        public static void InitalizeList()
        {
            InitalizeDrive();
            InitalizePartition();
            Initalized = true;
        }

        /// <summary>
        /// 初始化磁盘列表。
        /// </summary>
        internal static void InitalizePartition()
        {
            _allPartitions = Array.AsReadOnly(DriveDetector.GetPartitions());
            PartitionInitalized = true;
        }

        /// <summary>
        /// 初始化驱动器列表。
        /// </summary>
        internal static void InitalizeDrive()
        {
            _allDrives = Array.AsReadOnly(DriveDetector.GetDrives());
            DriveInitalized = true;
        }
    }
}
