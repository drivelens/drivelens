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
