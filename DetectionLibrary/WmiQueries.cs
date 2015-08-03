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
    }
}
