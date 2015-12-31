using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management;

namespace Drivelens.DetectionLibrary
{
    /// <summary>
    /// 表示一个驱动器。
    /// </summary>
    public sealed class DriveInfo
    {
        /// <summary>
        /// 用指定的 Win32_DiskDrive WMI 对象初始化 DriveInfo 对象的新实例。
        /// </summary>
        /// <param name="source"></param>
        internal DriveInfo(ManagementObject source)
        {
            this.Model = source.GetConvertedProperty("Model", Convert.ToString, null);
            this.DeviceId = source.GetConvertedProperty("DeviceId", Convert.ToString, null);
            this.InterfaceType = source.GetConvertedProperty("InterfaceType", Convert.ToString, null);
            this.Capacity = source.GetConvertedProperty("DeviceId", Convert.ToInt64, -1);
            this.SerialNumber = source.GetConvertedProperty("SerialNumber", s => Convert.ToString(s).Trim(), null);
            this.Firmware = source.GetConvertedProperty("FirmwareRevision", Convert.ToString, null);
            this.Index = source.GetConvertedProperty("Index", Convert.ToInt32, -1);

            DiskControllerInfo? info = DiskInformationUtility.GetDiskControllerInfo(source);
            this.ControllerName = info?.ControllerName;
            this.ControllerService = info?.ControllerService;
        }

        #region 属性
        /// <summary>
        /// 获取此驱动器的控制器名称。
        /// </summary>
        public string ControllerName { get; private set; }

        /// <summary>
        /// 获取此驱动器的控制器服务名称。
        /// </summary>
        public string ControllerService { get; private set; }

        /// <summary>
        /// 获取此驱动器的路径。
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// 获取此驱动器的固件版本。
        /// </summary>
        public string Firmware { get; private set; }

        /// <summary>
        /// 获取此驱动器的型号。
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// 获取此驱动器的的序列号。
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// 获取此驱动器的容量。
        /// </summary>
        public long Capacity { get; private set; }

        /// <summary>
        /// 获取此驱动器的接口类型。
        /// </summary>
        public string InterfaceType { get; private set; }

        /// <summary>
        /// 获取此驱动器的序号。
        /// </summary>
        public int Index { get; private set; }

        #endregion
    }


}
