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
    public class DriveInfo : WmiDeviceInfoObjectBase
    {
        public static ReadOnlyPoolCollection<DriveInfo, string> LocalDrives
        {
            get
            {
                return new ReadOnlyPoolCollection<DriveInfo, string>(WmiUtility.GetAllDiskDrives().Select(mobject => DriveInfo.Get(mobject)).ToList());
            }
        }

        public static DriveInfo Get(string id) =>
            ReadOnlyPoolCollection<DriveInfo, string>
                .GetOrCreate(id,
                () => new DriveInfo(WmiUtility.GetDiskDriveObjectById(id)));

        internal static DriveInfo Get(ManagementObject source) =>
            ReadOnlyPoolCollection<DriveInfo, string>
                .GetOrCreate(source.GetConvertedProperty("DeviceId", Convert.ToString),
                () => new DriveInfo(source));

        /// <summary>
        /// 用指定的 WMI 对象（Win32_DiskDrive）初始化 DriveInfo 对象的新实例。
        /// </summary>
        /// <param name="source">用于初始化的 WMI 对象（Win32_DiskDrive）。</param>
        protected DriveInfo(ManagementObject source) : base(source)
        {
            
        }

        /// <summary>
        /// 刷新本实例所包含的磁盘信息。
        /// </summary>
        public override void RefreshProperties()
        {
            RefreshPropertiesFromWmiObject(WmiUtility.GetDiskDriveObjectById(this.DeviceId));
        }

        /// <summary>
        /// 刷新磁盘信息。
        /// </summary>
        /// <param name="source">用于获取信息的 WMI 对象（Win32_DiskDrive）。</param>
        protected override void RefreshPropertiesFromWmiObject(ManagementObject source)
        {
            base.RefreshPropertiesFromWmiObject(source);
            this.Model = source.GetConvertedProperty("Model", Convert.ToString, null);
            this.InterfaceType = source.GetConvertedProperty("InterfaceType", Convert.ToString, null);
            this.Capacity = source.GetConvertedProperty("Size", Convert.ToInt64, -1);
            this.SerialNumber = source.GetConvertedProperty("SerialNumber", s => Convert.ToString(s).Trim(), null);
            this.Firmware = source.GetConvertedProperty("FirmwareRevision", Convert.ToString, null);
            this.Index = source.GetConvertedProperty("Index", Convert.ToInt32, -1);

            DiskControllerInfo? info = DiskInformationUtility.GetDiskControllerInfo(source);
            this.ControllerName = info?.ControllerName;
            this.ControllerService = info?.ControllerService;

            this.Partitions = new Lazy<ReadOnlyPoolCollection<DiskPartitionInfo, string>>(() =>
                new ReadOnlyPoolCollection<DiskPartitionInfo, string>(
                    WmiUtility.GetPartitionsByDiskDriveId(this.DeviceId)
                    .Select(mobj => DiskPartitionInfo.Get(mobj, this.DeviceId))
                    .ToList()));
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

        public Lazy<ReadOnlyPoolCollection<DiskPartitionInfo,string>> Partitions
        {
            get; private set;
        }

    }


}
