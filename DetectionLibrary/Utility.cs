using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    internal static class Utility
    {
        /// <summary>
        /// 根据指定的盘符获取 Win32_DiskPartition 对象。
        /// </summary>
        /// <param name="deviceId">盘符。</param>
        /// <returns>获取的 Win32_DiskPartition 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394175.aspx"/>
        internal static ManagementObject GetDiskPartitionObjectFromDeviceId(string deviceId)
        {
            return GetFirstObjectOrNull(string.Format(WmiQueries.DeviceIdToDiskPartition, deviceId));
        }

        /// <summary>
        /// 根据指定的盘符获取卷（Win32_Volume）对象。
        /// </summary>
        /// <param name="deviceId">盘符。</param>
        /// <returns>获取的 Win32_Volume 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394515.aspx"/>
        internal static ManagementObject GetVolumeObjectByDeviceId(string deviceId)
        {
            return GetFirstObjectOrNull(string.Format(WmiQueries.DeviceIdToWin32Volume, deviceId));
        }

        /// <summary>
        /// 根据指定的磁盘分区设备号（Win32_DiskPartition.DeviceID）获取磁盘（Win32_DiskDrive）对象。
        /// </summary>
        /// <param name="partitionId">磁盘分区设备号（Win32_DiskPartition.DeviceID）。</param>
        /// <returns>获取的 Win32_DiskDrive 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394135.aspx"/>
        internal static ManagementObject GetDiskDriveObjectByDiskPartitionId(string partitionId)
        {
            return GetFirstObjectOrNull(string.Format(WmiQueries.PartitionIdToDiskDrive, partitionId));
        }

        /// <summary>
        /// 根据指定的 WMI 查询语句来执行 WMI 查询，如果查询到则返回第一个对象，如果没有查询到则返回 null。
        /// </summary>
        /// <param name="query">要使用的 WMI 查询语句。</param>
        /// <returns>查询结果。如果查询到对象则返回查询到的对象；如果没有查询到则返回 null。</returns>
        internal static ManagementObject GetFirstObjectOrNull(string query)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var resultObjects = searcher.Get().Cast<ManagementObject>();
                return resultObjects.Count() == 0 ? null : resultObjects.First();
            }
        }
    }
}
