using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary
{
    public class DiskPartitionInfo : WmiDeviceInfoObjectBase
    {
        public static DiskPartitionInfo Get(string deviceId) =>
            ReadOnlyPoolCollection<DiskPartitionInfo, string>
                .GetOrCreate(deviceId, () => new DiskPartitionInfo(WmiUtility.GetDiskPartitionObjectById(deviceId)));

        internal static DiskPartitionInfo Get(ManagementObject source) =>
            ReadOnlyPoolCollection<DiskPartitionInfo, string>
            .GetOrCreate(source.GetConvertedProperty("DeviceId", Convert.ToString),
                () => new DiskPartitionInfo(source));

        protected DiskPartitionInfo(ManagementObject source) : base(source)
        {
            this.Drive = new Lazy<DriveInfo>(() =>
                 DriveInfo.Get(
                     WmiUtility.GetDiskDriveObjectByPartitionId(this.DeviceId)));
        }

        public override void RefreshProperties()
        {
            RefreshPropertiesFromWmiObject(WmiUtility.GetDiskPartitionObjectById(this.DeviceId));
        }

        protected override void RefreshPropertiesFromWmiObject(ManagementObject source)
        {
            base.RefreshPropertiesFromWmiObject(source);
            this.Index = source.GetConvertedProperty("Index", Convert.ToInt32, -1);
            this.StartingOffset = source.GetConvertedProperty("StartingOffset", Convert.ToInt64, -1);
            this.PrimaryPartition = source.GetConvertedProperty("PrimaryPartition", Convert.ToBoolean);
        }

        #region 属性
        public int Index { get; private set; }
        public long StartingOffset { get; private set; }
        public bool PrimaryPartition { get; private set; }
        #endregion

        public Lazy<DriveInfo> Drive { get; private set; }
    }
}
