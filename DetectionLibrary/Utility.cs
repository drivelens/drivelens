using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary
{
    public static class Utility
    {
        /// <summary>
        /// 根据指定的盘符获取 Win32_DiskPartition 对象。
        /// </summary>
        /// <param name="deviceId">盘符。</param>
        /// <returns>获取的 Win32_DiskPartition 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394175.aspx"/>
        public static ManagementObject GetDiskPartitionObjectByDeviceId(string deviceId) =>
            GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{deviceId}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");

        /// <summary>
        /// 根据指定的盘符获取卷（Win32_Volume）对象。
        /// </summary>
        /// <param name="deviceId">盘符。</param>
        /// <returns>获取的 Win32_Volume 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394515.aspx"/>
        public static ManagementObject GetVolumeObjectByDeviceId(string deviceId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_Volume WHERE DriveLetter = '{deviceId}'");

        /// <summary>
        /// 根据指定的磁盘分区设备号（Win32_DiskPartition.DeviceID）获取磁盘（Win32_DiskDrive）对象。
        /// </summary>
        /// <param name="partitionId">磁盘分区设备号（Win32_DiskPartition.DeviceID）。</param>
        /// <returns>获取的 Win32_DiskDrive 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394135.aspx"/>
        public static ManagementObject GetDiskDriveObjectByDiskPartitionId(string partitionId) =>
            GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");

        /// <summary>
        /// 根据指定的 WMI 查询语句来执行 WMI 查询，如果查询到则返回第一个对象，如果没有查询到则返回 null。
        /// </summary>
        /// <param name="query">要使用的 WMI 查询语句。</param>
        /// <returns>查询结果。如果查询到对象则返回查询到的对象；如果没有查询到则返回 null。</returns>
        public static ManagementObject GetFirstObjectOrNull(string query)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var resultObjects = searcher.Get().Cast<ManagementObject>();
                return resultObjects.Count() == 0 ? null : resultObjects.First();
            }
        }
    }
}
