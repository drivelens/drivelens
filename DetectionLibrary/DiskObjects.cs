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
        internal static bool _isPartitionInitalized;

        internal static bool _isDriveInitalized;

        internal static bool _isInitalized;

        private static ReadOnlyCollection<PartitionInfo> _allPartitions;

        private static ReadOnlyCollection<DriveInfo> _allDrives;

        internal static ReadOnlyCollection<PartitionInfo> PartitionsInternal
        {
            get
            {
                return _allPartitions;
            }
        }

        public static ReadOnlyCollection<PartitionInfo> AllPartitions
        {
            get
            {
                if(!_isInitalized)
                {
                    throw new InvalidOperationException();
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

        public static ReadOnlyCollection<DriveInfo> AllDrives
        {
            get
            {
                if (!_isInitalized)
                {
                    throw new InvalidOperationException();
                }
                return _allDrives;
            }
        }

        public static void InitalizeList()
        {
            InitalizeDrive();
            InitalizePartition();
            _isInitalized = true;
        }

        internal static void InitalizePartition()
        {
            _allPartitions = Array.AsReadOnly(DriveDetector.GetPartitions());
            _isPartitionInitalized = true;
        }

        internal static void InitalizeDrive()
        {
            _allDrives = Array.AsReadOnly(DriveDetector.GetDrives());
            _isDriveInitalized = true;
        }
    }
}
