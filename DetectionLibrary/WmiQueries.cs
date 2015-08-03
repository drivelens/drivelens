using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    internal static class WmiQueries
    {
        /// <summary>
        /// 获取所有磁盘分区对象的 WMI 查询语句，DriveType 值为 2 时是可移动磁盘，值为 3 是本地磁盘。
        /// Win32_LogicalDisk 对象参考：https://msdn.microsoft.com/en-us/library/aa394173.aspx
        /// </summary>
        public static readonly string AllLocalLogicalDisk = @"SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 OR DriveType = 3";

        /// <summary>
        /// 根据指定的盘符获取卷（Win32_Volume）对象的 WMI 查询语句。
        /// </summary>
        public static readonly string DeviceIdToWin32Volume = @"SELECT * FROM Win32_Volume WHERE DriveLetter = 'C:'";

        /// <summary>
        /// 根据指定的盘符获取磁盘分区（Win32_DiskPartition）对象的 WMI 查询语句。
        /// </summary>
        public static readonly string DeviceIdToDiskPartition = @"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{0}'}} WHERE AssocClass = Win32_LogicalDiskToPartition";

        /// <summary>
        /// 根据指定的磁盘分区设备号（Win32_DiskPartition.DeviceID）获取磁盘（Win32_DiskDrive）对象的 WMI 查询语句。
        /// </summary>
        public static readonly string PartitionIdToDiskDrive = @"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{0}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
    }
}
