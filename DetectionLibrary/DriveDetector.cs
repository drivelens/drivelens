using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    public class DriveDetector
    {
        public static PartitionInfo[] GetPartitions()
        {
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk"))
            {
                return logicalDiskSearcher.Get().Cast<ManagementObject>()
                    .Select(CreatePartitionInfoFromLogicalDiskObject).ToArray();
            }
        }

        public static PartitionInfo CreatePartitionInfoFromLogicalDiskObject(ManagementObject logicalDiskObject)
        {
            PartitionInfo partition = new PartitionInfo();
            partition.Capacity = (long)(ulong)logicalDiskObject["Size"];
            partition.FreeSpace = (long)(ulong)logicalDiskObject["FreeSpace"];
            partition.PartionType = (int)(uint)logicalDiskObject["DriveType"];
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();
            ManagementObject volumeObject = Utility.GetVolumeObjectFromDeviceId(partition.DeviceId);
            if (volumeObject != null)
            {
                partition.BlockSize = (long)(ulong)volumeObject["BlockSize"];
            }
            ManagementObject diskPartitionObject = Utility.GetDiskPartitionObjectFromDeviceId(partition.DeviceId);
            if (diskPartitionObject != null)
            {
                partition.Index = (int)(uint)diskPartitionObject["Index"];
                partition.StartingOffset = (long)(ulong)diskPartitionObject["StartingOffset"];
            }
            return partition;
        }
    }
}
