using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary
{
    public static class DriveDetector
    {
        /// <summary>
        /// 获取该计算机上的所有磁盘分区。
        /// </summary>
        /// <returns>计算机上的所有磁盘分区。</returns>
        internal static PartitionInfo[] GetPartitions()
        {
            if (!DiskObjects.DriveInitalized)
            {
                DiskObjects.InitalizeDrive();
            }
            return WmiUtility.GetAllLocalDisks()                          // 获取所有的 LogicalDisk 对象。
                .Select(m => new PartitionInfo(m))         // 转换为 PartitionInfo 数组。
                .ToArray();
        }

        /// <summary>
        /// 获取该计算机上的所有磁盘驱动器。
        /// </summary>
        /// <returns>计算机上的所有磁盘驱动器。</returns>
        internal static DriveInfo[] GetDrives()
        {
            return WmiUtility.GetAllLocalDrives()                          // 获取所有的 LogicalDisk 对象。
                .Select(m => new DriveInfo(m))         // 转换为 PartitionInfo 数组。
                .ToArray();
        }
    }
}
