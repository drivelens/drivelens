using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
            partition.Capacity = (long)logicalDiskObject["Size"];
            partition.FreeSpace = (long)logicalDiskObject["FreeSpace"];
            partition.PartionType = (int)logicalDiskObject["DriveType"];
            partition.FileSystem = (string)logicalDiskObject["FileSystem"];
            partition.DeviceId = (string)logicalDiskObject["DeviceID"];
            partition.VolumeName = (string)logicalDiskObject["VolumeName"];
            partition.SerialNumber = ((string)logicalDiskObject["VolumeSerialNumber"]).Trim();
            ManagementObject volumeObject = Utility.GetVolumeObjectFromDeviceId(partition.DeviceId);
            partition.BlockSize = (long)volumeObject["BlockSize"];
            ManagementObject diskPartitionObject = Utility.GetDiskPartitionObjectFromDeviceId(partition.DeviceId);
            partition.Index = (int)(uint)diskPartitionObject["Index"];
            partition.StartingOffset = (long)diskPartitionObject["StartingOffset"];
            return partition;
        }
    }
}
