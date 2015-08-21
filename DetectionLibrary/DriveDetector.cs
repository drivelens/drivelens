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
            if (!DiskObjects.DriveInitalized)
            {
                DiskObjects.InitalizeDrive();
            }
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

        /// <summary>
        /// 根据 Win32_DiskDrive WMI 对象创建 DriveInfo 对象。
        /// </summary>
        /// <param name="diskDriveObject">表示 Win32_DiskDrive 的 WMI 对象。</param>
        /// <returns>创建的 DriveInfo 对象。</returns>
        private static DriveInfo CreateDriveInfoFromDiskDriveObject(ManagementObject diskDriveObject)
        {
            DriveInfo drive = new DriveInfo();

            // 基础信息
            drive.Model = diskDriveObject["Model"].ToString();
            drive.DeviceId = (string)diskDriveObject["DeviceID"];
            drive.InterfaceType = (string)diskDriveObject["InterfaceType"];
            drive.Capacity = diskDriveObject["Size"] != null ? (long)(ulong)diskDriveObject["Size"] : 0;
            drive.SerialNumber = ((string)diskDriveObject["SerialNumber"]).Trim();
            drive.Firmware = (string)diskDriveObject["FirmwareRevision"];
            drive.Index = (int)(uint)diskDriveObject["Index"];

            //控制器信息
            DiskControllerNameInfo? controllerInfo = DiskInformationUtility.GetDiskControllerNameService(diskDriveObject);
            drive.ControllerName = controllerInfo?.ControllerName;
            drive.ControllerService = controllerInfo?.ControllerService;

            return drive;
        }

        /// <summary>
        /// 根据 Win32_LogicalDisk WMI 对象创建 PartitionInfo 对象。
        /// </summary>
        /// <param name="logicalDiskObject">表示 Win32_LogicalDisk 的 WMI 对象。</param>
        /// <returns>创建的 PartitionInfo 对象。</returns>
        public static PartitionInfo CreatePartitionInfoFromLogicalDiskObject(ManagementObject logicalDiskObject)
        {
            PartitionInfo partition = new PartitionInfo();

            // 基础信息
            partition.Capacity = (long)(ulong)logicalDiskObject["Size"];            // 分区大小
            partition.FreeSpace = (long)(ulong)logicalDiskObject["FreeSpace"];      // 剩余空间
            partition.PartitionType = (int)(uint)logicalDiskObject["DriveType"];    // 磁盘类型
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];         // 文件系统
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];             // 盘符
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];         // 卷标
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();  // 分区序列号，头尾去除空格

            // 卷信息
            partition.BlockSize = DiskInformationUtility.GetPartitionBlockSize(partition.DeviceId); // 分区的分配单元大小

            DiskPartitionInfo? indexAndStartingOffset = DiskInformationUtility.GetDiskPartitionIndexAndStartingOffset(partition.DeviceId);
            partition.Index = indexAndStartingOffset?.Index;                        // 分区的索引
            partition.StartingOffset = indexAndStartingOffset?.StartingOffset;      // 分区起始偏移

            if(indexAndStartingOffset != null && DiskObjects.DriveInitalized) // 如果获取到相应的分区，那么说明有相应的磁盘，如果磁盘此时已经加载就返回磁盘信息。
            {
                ManagementObject diskDriveObject = WmiUtility.GetDiskDriveObjectByPartitionId(indexAndStartingOffset?.DeviceId);
                // 此处初始化磁盘时 Drive 里的磁盘信息不完善，不能让外部访问，故创建此特殊通道。
                DriveInfo drive = DiskObjects.DrivesInternal.Where(disk => disk.DeviceId == (string)diskDriveObject["DeviceId"]).First();
                drive.AddPartition(partition);
                partition.Drive = drive;
            }

            return partition;
        }


    }

}
