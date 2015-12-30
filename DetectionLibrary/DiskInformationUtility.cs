using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace DiskMagic.DetectionLibrary
{
    public static class DiskInformationUtility
    {
        /// <summary>
        /// 获取指定磁盘的单元大小。
        /// </summary>
        /// <param name="deviceId">该磁盘的盘符（例如 D:）。</param>
        /// <returns>如果获取到，则为该磁盘的单元大小；如果没有获取到则为Null。</returns>
        public static long? GetPartitionBlockSize(string deviceId)
        {
            // 获取卷对象用于获取分区的区块大小。
            using (ManagementObject volumeObject = WmiUtility.GetVolumeObjectByDeviceId(deviceId))
                // 如果获取到了就设置，如果没有获取到就设为 -1。
                if (volumeObject != null)
                {
                    return (long)(ulong)volumeObject["BlockSize"];       // 分区的区块大小。
                }
                else
                {
                    return null;
                }
        }

        /// <summary>
        /// 获取指定磁盘的序号和起始偏移。
        /// </summary>
        /// <param name="deviceId">该分区的盘符（例如 D:）。</param>
        /// <returns>如果该磁盘有对应的分区则为获取到的值；如果没有则为 Null。</returns>
        public static DiskPartitionInfo? GetDiskPartitionIndexAndStartingOffset(string deviceId)
        {
            using (ManagementObject diskPartitionObject = WmiUtility.GetDiskPartitionObjectByDeviceId(deviceId))
                if (diskPartitionObject != null)
                {
                    DiskPartitionInfo result = new DiskPartitionInfo();
                    result.Index = (int)(uint)diskPartitionObject["Index"];
                    result.StartingOffset = (long)(ulong)diskPartitionObject["StartingOffset"];
                    result.DeviceId = (string)diskPartitionObject["DeviceId"];
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
                result.ControllerName = (string)controllerDeviceObject["Name"];
                using (ManagementObject controllerPnPObject = WmiUtility.GetPnPEntityObjectByPnPDeviceId((string)controllerDeviceObject["DeviceId"]))
                {
                    result.ControllerService = (string)controllerPnPObject["Service"];
                    // 在 IDE 接口的电脑上需要进行两次该操作才能获取到真实的控制器信息，否则只能获得“主要 IDE 通道”字样。
                    if(result.ControllerService == "atapi")
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
