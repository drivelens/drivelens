using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    public static class Utility
    {
        public static PartitionInfo CreatePartitionInfoFromLogicalDiskObject(ManagementObject logicalDiskObject)
        {
            PartitionInfo partition = new PartitionInfo();
            partition.Capacity = (ulong)logicalDiskObject["Size"];
            partition.FreeSpace = (ulong)logicalDiskObject["FreeSpace"];
            partition.PartionType = (uint)logicalDiskObject["DriveType"];
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();
            ManagementObject volumeObject = GetVolumeObjectFromDeviceId(partition.DeviceId);
            partition.BlockSize = (ulong)volumeObject["BlockSize"];
            ManagementObject diskPartitionObject = GetDiskPartitionObjectFromDeviceId(partition.DeviceId);
            partition.Index = (int)(uint)diskPartitionObject["Index"];
            partition.StartingOffset = (ulong)diskPartitionObject["StartingOffset"];
            return partition;
        }

        public static ManagementObject GetDiskPartitionObjectFromDeviceId(string partitionId)
        {
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + partitionId + "'} WHERE AssocClass = Win32_LogicalDiskToPartition"))
            {
                return logicalDiskSearcher.Get().Cast<ManagementObject>().First();
            }
        }

        public static ManagementObject GetVolumeObjectFromDeviceId(string partitionId)
        {
            using (ManagementObjectSearcher volumeSearcher = new ManagementObjectSearcher("Select * from Win32_Volume Where Name = '" + partitionId + @"\\'"))
            {
                return volumeSearcher.Get().Cast<ManagementObject>().First();
            }
        }
    }
}
