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
                    .Select(Utility.CreatePartitionInfoFromLogicalDiskObject).ToArray();
            }
        }
    }
}
