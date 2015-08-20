using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace DiskMagic.DetectionLibrary
{
    public static class WmiUtility
    {
        /// <summary>
        /// 获取所有的 LogicalDisk 对象。
        /// </summary>
        /// <returns>获取到的 LogicalDisk 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394173.aspx"/>
        public static IEnumerable<ManagementObject> GetAllLocalDisks()
        {
            // 获取所有磁盘分区对象的 WMI 查询语句，DriveType 值为 2 时是可移动磁盘，值为 3 是本地磁盘。
            using (ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 OR DriveType = 3"))
                return logicalDiskSearcher.Get().Cast<ManagementObject>();
        }

        /// <summary>
        /// 获取所有的 DiskDrive 对象。
        /// </summary>
        /// <returns>获取到的 LogicalDisk 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394173.aspx"/>
        public static IEnumerable<ManagementObject> GetAllLocalDrives()
        {
            // 获取所有磁盘分区对象的 WMI 查询语句，DriveType 值为 2 时是可移动磁盘，值为 3 是本地磁盘。
            using (ManagementObjectSearcher diskDriveSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
                return diskDriveSearcher.Get().Cast<ManagementObject>();
        }

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
        /// 根据指定的磁盘分区设备序号（Win32_DiskPartition.DeviceId）获取磁盘（Win32_DiskDrive）对象。
        /// </summary>
        /// <param name="partitionId">磁盘分区设备号（Win32_DiskPartition.DeviceID）。</param>
        /// <returns>获取的 Win32_DiskDrive 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394135.aspx"/>
        public static ManagementObject GetDiskDriveObjectByPartitionId(string partitionId) =>
            GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");

        /// <summary>
        /// 根据指定的 Win32_DiskDrive 对象返回 IDE 控制器（Win32_IDEControllerDevice）对象。
        /// </summary>
        /// <param name="pnpDeviceId"></param>
        /// <returns></returns>
        public static ManagementObject GetControllerDeviceByDiskDriveObject(ManagementObject diskDriveObject)
        {
            string controllerObjectName;
            switch ((string)diskDriveObject["InterfaceType"])
            {
                case "IDE":
                    controllerObjectName = "Win32_IDEControllerDevice";
                    break;
                case "SCSI":
                    controllerObjectName = "Win32_SCSIControllerDevice";
                    break;
                case "USB":
                    controllerObjectName = "Win32_USBControllerDevice";
                    break;
                default:
                    return null;
            }

            return GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_PNPEntity.DeviceID='{(string)diskDriveObject["PNPDeviceID"]}'}} WHERE AssocClass = {controllerObjectName}");
        }

        /// <summary>
        /// 根据指定的 PnP 实体对象 ID（Win32_PnPEntity.DeviceID）获取 Win32_PnPEntity 对象。
        /// </summary>
        /// <param name="pnpDeviceId">PnP 实体对象 ID。</param>
        /// <returns>获取到的 Win32_PnPEntity 对象。</returns>
        public static ManagementObject GetPnPEntityObjectByPnPDeviceId(string pnpDeviceId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_PnPEntity WHERE DeviceID = {pnpDeviceId.Replace(@"\", @"\\")}");

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
