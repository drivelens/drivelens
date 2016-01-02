using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary.Wmi
{
    /// <summary>
    /// 操作 WMI 对象。
    /// </summary>
    public static class WmiUtility
    {
        /// <summary>
        /// 获取所有本地逻辑分区（Win32_LogicalDisk）对象。获取到的对象均为本地磁盘或可移动磁盘（DriveType = 2 或 DriveType = 3）。
        /// </summary>
        /// <returns>所获取的 Win32_LogicalDisk 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394173.aspx"/>
        public static IEnumerable<ManagementObject> GetAllLogicalDisks() =>
            // 获取所有磁盘分区对象的 WMI 查询语句，DriveType 值为 2 时是可移动磁盘，值为 3 是本地磁盘。
            GetObjects("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 2 OR DriveType = 3");
           

        /// <summary>
        /// 获取所有本地磁盘驱动器（Win32_DiskDrive）对象。
        /// </summary>
        /// <returns>获取到的 Win32_DiskDrive 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394173.aspx"/>
        public static IEnumerable<ManagementObject> GetAllDiskDrives() =>
            GetObjects("SELECT * FROM Win32_DiskDrive");

        /// <summary>
        /// 获得指定的磁盘驱动器（Win32_DiskDrive）包含的所有物理分区。
        /// </summary>
        /// <param name="diskDriveId">磁盘驱动器 ID（Win32_DiskDrive.DeviceId）。</param>
        /// <returns>该磁盘驱动器下的所有物理分区。</returns>
        public static IEnumerable<ManagementObject> GetPartitionsByDiskDriveId(string diskDriveId) =>
            GetObjects($"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{diskDriveId}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");

        /// <summary>
        /// 获得指定的物理分区（Win32_DiskPartition）包含的所有逻辑分区。
        /// </summary>
        /// <param name="partitionId">物理分区 ID（Win32_DiskPartition.DeviceId）</param>
        /// <returns>该物理分区下的所有逻辑分区。</returns>
        public static IEnumerable<ManagementObject> GetLogicalDisksByPartitionId(string partitionId) =>
            GetObjects($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");

        /// <summary>
        /// 获取指定的盘符（Win32_LogicalDisk.DeviceId）所对应的分区（Win32_DiskPartition）对象。
        /// </summary>
        /// <param name="deviceId">盘符（Win32_LogicalDisk.DeviceId）。</param>
        /// <returns>获取的 Win32_DiskPartition 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394175.aspx"/>
        public static ManagementObject GetDiskPartitionObjectByLogicalDiskDeviceId(string deviceId) =>
            GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{deviceId}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");

        /// <summary>
        /// 获取指定的盘符（Win32_LogicalDisk.DeviceId）所对应的卷（Win32_Volume）对象。
        /// </summary>
        /// <param name="deviceId">盘符（Win32_LogicalDisk.DeviceId）。</param>
        /// <returns>获取的 Win32_Volume 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394515.aspx"/>
        public static ManagementObject GetVolumeObjectByDeviceId(string deviceId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_Volume WHERE DriveLetter = '{deviceId}'");

        /// <summary>
        /// 获取指定的物理分区（Win32_DiskPartition）所对应的磁盘（Win32_DiskDrive）对象。
        /// </summary>
        /// <param name="partitionId">物理分区设备标识符（Win32_DiskPartition.DeviceID）。</param>
        /// <returns>获取的 Win32_DiskDrive 对象。</returns>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa394135.aspx"/>
        public static ManagementObject GetDiskDriveObjectByPartitionId(string partitionId) =>
            GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionId}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");

        /// <summary>
        /// 获取一个磁盘（Win32_DiskDrive）对象。
        /// </summary>
        /// <param name="diskDriveId">磁盘设备标识符（Win32_DiskDrive.DeviceId）。</param>
        /// <returns>获取的 Win32_DiskDrive 对象。</returns>
        public static ManagementObject GetDiskDriveObjectById(string diskDriveId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_DiskDrive WHERE DeivceId={diskDriveId}");

        /// <summary>
        /// 获取一个逻辑分区（Win32_LogicalDisk）对象。
        /// </summary>
        /// <param name="diskDriveId">分区标识符（Win32_LogicalDisk.DeviceId）。</param>
        /// <returns>获取的 Win32_LogicalDisk 对象。</returns>
        public static ManagementObject GetLogicalDiskObjectById(string logicalDiskId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_LogicalDisk WHERE DeivceId={logicalDiskId}");

        /// <summary>
        /// 获得一个物理分区（Win32_DiskPartition）对象。
        /// </summary>
        /// <param name="diskPartitionId">分区标识符（Win32_DiskPartition.DeviceId）。</param>
        /// <returns>获取的 Win32_DiskPartition 对象。</returns>
        public static ManagementObject GetDiskPartitionObjectById(string diskPartitionId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_DiskPartition WHERE DeivceId={diskPartitionId}");

        /// <summary>
        /// 获取指定的磁盘（Win32_DiskDrive）对象的控制器对象。
        /// 所获取的控制器对象类型根据磁盘类型而定，可能为 Win32_IDEControllerDevice、Win32_SCSIControllerDevice 和 Win32_USBControllerDevice 三者之一。
        /// </summary>
        /// <param name="diskDriveObject">要获取其控制器的磁盘（Win32_DiskDrive）对象。</param>
        /// <returns>获取到的控制器对象。</returns>
        public static ManagementObject GetControllerDeviceByDiskDriveObject(ManagementObject diskDriveObject)
        {
            string controllerObjectName;
            switch (diskDriveObject.GetConvertedProperty("InterfaceType", Convert.ToString, null))
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

            return GetFirstObjectOrNull($"ASSOCIATORS OF {{Win32_PNPEntity.DeviceID='{diskDriveObject.GetConvertedProperty("PNPDeviceId", Convert.ToString, "")}'}} WHERE AssocClass = {controllerObjectName}");
        }

        /// <summary>
        /// 获取一个 PnP 实体对象（Win32_PnPEntity）。
        /// </summary>
        /// <param name="pnpDeviceId">PnP 实体对象 ID（Win32_PnPEntity.DeviceID）。</param>
        /// <returns>获取到的 Win32_PnPEntity 对象。</returns>
        public static ManagementObject GetPnPEntityObjectByPnPDeviceId(string pnpDeviceId) =>
            GetFirstObjectOrNull($"SELECT * FROM Win32_PnPEntity WHERE DeviceID = '{pnpDeviceId.Replace(@"\", @"\\")}'");

        /// <summary>
        /// 根据指定的 WMI 查询语句执行 WMI 查询，并返回查询结果的第一项。
        /// </summary>
        /// <param name="query">要执行的 WMI 查询。</param>
        /// <returns>查询结果。如果查询到对象则返回查询到的第一项；如果没有查询到则返回 null。</returns>
        public static ManagementObject GetFirstObjectOrNull(string query)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var resultObjects = searcher.Get().Cast<ManagementObject>();
                return resultObjects.Count() == 0 ? null : resultObjects.First();
            }
        }

        /// <summary>
        /// 根据指定的 WMI 查询语句执行 WMI 查询，并返回查询结果。
        /// </summary>
        /// <param name="query">要执行的 WMI 查询。</param>
        /// <returns>查询到的对象。</returns>
        public static IEnumerable<ManagementObject> GetObjects(string query)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                return searcher.Get().Cast<ManagementObject>();
            }
        }

        /// <summary>
        /// 使用指定的转换函数，将指定对象转换为要求的类型。如果要转换的对象为 null，则返回指定的默认值。
        /// </summary>
        /// <typeparam name="TResult">目标类型。</typeparam>
        /// <param name="source">要转换的对象。</param>
        /// <param name="cast">用于转换的函数。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果要转换的对象不为 null，则调用指定的转换函数进行转换；否则返回默认值。</returns>
        public static TResult TryCastObject<TResult>(object source, Func<object, TResult> cast, TResult defaultValue = default(TResult))
        {
            if(source == null)
            {
                return defaultValue;
            }
            else
            {
                return cast(source);
            }
        }

        /// <summary>
        /// 尝试获得一个 WMI 对象指定的属性。如果值为 null，则返回指定的默认值。
        /// </summary>
        /// <typeparam name="TResult">目标类型。</typeparam>
        /// <param name="source">源 WMI 对象。</param>
        /// <param name="propertyName">相关的属性的名称。</param>
        /// <param name="cast">用于转换的函数。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果指定属性不为 null，则调用指定的转换函数进行转换；否则返回默认值。</returns>
        public static TResult GetConvertedProperty<TResult>(this ManagementObject source, string propertyName, Func<object, TResult> cast, TResult defaultValue = default(TResult))
        {
            return TryCastObject(source[propertyName], cast, defaultValue);
        }
    }
}
