using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Drivelens.DetectionLibrary
{
    public static class DiskInformationUtility
    {
        /// <summary>
        /// 获取指定磁盘的单元大小。
        /// </summary>
        /// <param name="deviceId">该磁盘的盘符（例如 D:）。</param>
        /// <returns>如果获取到，则为该磁盘的单元大小；如果没有获取到则为Null。</returns>
        public static long GetPartitionBlockSize(string deviceId)
        {
            // 获取卷对象用于获取分区的区块大小。
            using (ManagementObject volumeObject = WmiUtility.GetVolumeObjectByDeviceId(deviceId))
                // 如果获取到了就设置，如果没有获取到就设为 -1。
                if (volumeObject != null)
                {
                    return volumeObject.GetConvertedProperty("BlockSize", Convert.ToInt64, -1);       // 分区的区块大小。
                }
                else
                {
                    return -1;
                }
        }

        /// <summary>
        /// 获取指定磁盘的序号和起始偏移。
        /// </summary>
        /// <param name="deviceId">该分区的盘符（例如 D:）。</param>
        /// <returns>如果该磁盘有对应的分区则为获取到的值；如果没有则为 Null。</returns>
        public static DiskPartitionInfo? GetDiskPartitionIndexAndStartingOffset(string deviceId)
        {
            using (ManagementObject diskPartitionObject = WmiUtility.GetDiskPartitionObjectByLogicalDiskDeviceId(deviceId))
                if (diskPartitionObject != null)
                {
                    DiskPartitionInfo result = new DiskPartitionInfo();
                    result.Index = diskPartitionObject.GetConvertedProperty("Index", Convert.ToInt32, -1);
                    result.StartingOffset = diskPartitionObject.GetConvertedProperty("StartingOffset", Convert.ToInt64, -1);
                    result.DeviceId = diskPartitionObject.GetConvertedProperty("DeviceId", Convert.ToString, null);
                    return result;
                }
                else
                {
                    return null;
                }
        }

        public static DiskControllerInfo? GetDiskControllerInfo(ManagementObject diskDriveObject)
        {
            DiskControllerInfo result = new DiskControllerInfo();
            using (ManagementObject controllerDeviceObject = WmiUtility.GetControllerDeviceByDiskDriveObject(diskDriveObject))
            {
                result.ControllerName = controllerDeviceObject.GetConvertedProperty("Name", Convert.ToString, null);
                using (ManagementObject controllerPnPObject = WmiUtility.GetPnPEntityObjectByPnPDeviceId(controllerDeviceObject.GetConvertedProperty("DeviceId", Convert.ToString, null)))
                {
                    result.ControllerService = controllerDeviceObject.GetConvertedProperty("Service", Convert.ToString, null);
                    // 在 IDE 接口的电脑上需要进行两次该操作才能获取到真实的控制器信息，否则只能获得“主要 IDE 通道”字样。
                    if (result.ControllerService == "atapi")
                    {
                        return GetDiskControllerInfo(controllerPnPObject);
                    }
                }
            }
            return result;
        }


    }
    public struct DiskPartitionInfo
    {
        public int Index;
        public long StartingOffset;
        public string DeviceId;
    }

    public struct DiskControllerInfo
    {
        public string ControllerName;
        public string ControllerService;
    }
}
