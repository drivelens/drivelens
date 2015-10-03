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
        public static PartitionInfo[] GetPartitions()
        {
            // 获取所有磁盘分区对象的 WMI 查询语句，DriveType 值为 2 时是可移动磁盘，值为 3 是本地磁盘。
            // Win32_LogicalDisk 对象参考：https://msdn.microsoft.com/en-us/library/aa394173.aspx
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 OR DriveType = 3"))
            {
                return logicalDiskSearcher.Get().Cast<ManagementObject>()   // 转换为 ManagementObject 枚举。
                    .Select(CreatePartitionInfoFromLogicalDiskObject)       // 转换为 PartitionInfo 数组。
                    .ToArray();
            }
        }

        /// <summary>
        /// 将表示 Win32_LogicalDisk 的 WMI 对象转换为 PartitionInfo 对象。
        /// </summary>
        /// <param name="logicalDiskObject">表示 Win32_LogicalDisk 的 WMI 对象。</param>
        /// <returns>转换后的 PartitionInfo 对象。</returns>
        public static PartitionInfo CreatePartitionInfoFromLogicalDiskObject(ManagementObject logicalDiskObject)
        {
            PartitionInfo partition = new PartitionInfo();

            partition.Capacity = (long)(ulong)logicalDiskObject["Size"];            // 分区大小
            partition.FreeSpace = (long)(ulong)logicalDiskObject["FreeSpace"];      // 剩余空间
            partition.PartitionType = (int)(uint)logicalDiskObject["DriveType"];             // 磁盘类型
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];         // 文件系统
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];             // 盘符
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];         // 卷标
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();  // 分区序列号，头尾去除空格

            // 获取卷对象用于获取分区的区块大小。
            ManagementObject volumeObject = Utility.GetVolumeObjectByDeviceId(partition.DeviceId);
            // 如果获取到了就设置，如果没有获取到就设为 -1。
            if (volumeObject != null)
            {
                partition.BlockSize = (long)(ulong)volumeObject["BlockSize"];       // 分区的区块大小。
            }
            else
            {
                partition.BlockSize = -1;
            }

            ManagementObject diskPartitionObject = Utility.GetDiskPartitionObjectByDeviceId(partition.DeviceId);
            if (diskPartitionObject != null)                                        // 此处同上。
            {
                partition.Index = (int)(uint)diskPartitionObject["Index"];          // 分区的索引。
                partition.StartingOffset = (long)(ulong)diskPartitionObject["StartingOffset"];  // 分区起始偏移。
            }
            else
            {
                partition.StartingOffset = partition.Index = -1;                    // 如果无法获取，就设置其为 -1。
            }

            return partition;
        }
    }
}
