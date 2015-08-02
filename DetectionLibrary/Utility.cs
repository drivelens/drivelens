using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    public class Utility
    {
        public static PartitionInfo[] GetPartitions()
        {
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk"))
            {
                return logicalDiskSearcher.Get().Cast<ManagementObject>()
                    .Select(CreatePartitionInfoFromLogicalDiskObject).ToArray();
            }
        }

        private static PartitionInfo CreatePartitionInfoFromLogicalDiskObject(ManagementObject logicalDiskObject)
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

        private static ulong GetPartitonBlockSize(string partitonId)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_Volume Where Name = '" + partitonId + @"\\'"))
            using (ManagementObject obj2 = searcher.Get().Cast<ManagementObject>().First())
            {
                return (ulong)obj2["BlockSize"];
            }
        }

        private static ManagementObject GetDiskPartitionObjectFromDeviceId(string partitionId)
        {
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + partitionId + "'} WHERE AssocClass = Win32_LogicalDiskToPartition"))
            {
                return logicalDiskSearcher.Get().Cast<ManagementObject>().First();
            }
        }

        private static ManagementObject GetVolumeObjectFromDeviceId(string partitionId)
        {
            using (ManagementObjectSearcher volumeSearcher = new ManagementObjectSearcher("Select * from Win32_Volume Where Name = '" + partitionId + @"\\'"))
            {
                return volumeSearcher.Get().Cast<ManagementObject>().First();
            }
        }
    }
}
