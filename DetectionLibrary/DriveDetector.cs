using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    public static class DriveDetector
    {
        /// <summary>
        /// 获取该计算机上的所有磁盘分区。
        /// </summary>
        /// <returns>计算机上的所有磁盘分区。</returns>
        public static PartitionInfo[] GetPartitions()
        {
            return WmiUtility.GetAllLocalDisks()                          // 获取所有的 LogicalDisk 对象。
                .Select(CreatePartitionInfoFromLogicalDiskObject)         // 转换为 PartitionInfo 数组。
                .ToArray();
        }

        /// <summary>
        /// 获取该计算机上的所有磁盘驱动器。
        /// </summary>
        /// <returns>计算机上的所有磁盘驱动器。</returns>
        internal static DriveInfo[] GetDrives()
        {
            
            return WmiUtility.GetAllLocalDrives()                          // 获取所有的 LogicalDisk 对象。
                .Select(CreateDriveInfoFromDiskDriveObject)         // 转换为 PartitionInfo 数组。
                .ToArray();
        }

        public static DriveInfo CreateDriveInfoFromDiskDriveObject(ManagementObject diskDriveObject)
        {
            DriveInfo drive = new DriveInfo();
            drive.Model = diskDriveObject["Model"].ToString();
            drive.DeviceId = (string)diskDriveObject["DeviceID"];
            drive.InterfaceType = (string)diskDriveObject["InterfaceType"];
            drive.Capacity = diskDriveObject["Size"] != null ? (long)(ulong)diskDriveObject["Size"] : 0;
            drive.SerialNumber = ((string)diskDriveObject["SerialNumber"]).Trim();
            drive.Firmware = (string)diskDriveObject["FirmwareRevision"];
            drive.Index = (int)(uint)diskDriveObject["Index"];
            DiskControllerNameInfo controllerInfo = new DiskControllerNameInfo();
            drive.ControllerName = controllerInfo.ControllerName;
            drive.ControllerService = controllerInfo.ControllerService;
            return drive;
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
            partition.PartitionType = (int)(uint)logicalDiskObject["DriveType"];    // 磁盘类型
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];         // 文件系统
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];             // 盘符
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];         // 卷标
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();  // 分区序列号，头尾去除空格

            partition.BlockSize = DiskInformationUtility.GetPartitionBlockSize(partition.DeviceId);

            DiskPartitionInfo? indexAndStartingOffset = DiskInformationUtility.GetDiskPartitionIndexAndStartingOffset(partition.DeviceId);
            partition.Index = indexAndStartingOffset?.Index;                        // 分区的索引。
            partition.StartingOffset = indexAndStartingOffset?.StartingOffset;      // 分区起始偏移。

            if(indexAndStartingOffset != null)
            {
                ManagementObject diskDriveObject = WmiUtility.GetDiskDriveObjectByPartitionId(indexAndStartingOffset?.DeviceId);
                if(!DiskObjects._isDriveInitalized)
                {
                    DiskObjects.InitalizeDrive();
                }
                DriveInfo drive = DiskObjects.DrivesInternal.Where(disk => disk.DeviceId == (string)diskDriveObject["DeviceId"]).First();
                drive.Partitions.Add(partition);
                partition.Drive = drive;
            }

            return partition;
        }


    }

}
