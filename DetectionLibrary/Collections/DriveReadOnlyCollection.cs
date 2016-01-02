using Drivelens.DetectionLibrary.Objects;
using Drivelens.DetectionLibrary.Wmi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary.Collections
{
    class DriveReadOnlyCollection : ReadOnlyPoolCollection<DriveInfo, string>
    {
        internal DriveReadOnlyCollection(List<DriveInfo> objects) : base(objects) { }

        public static DriveInfo Get(string id) =>
            GetOrCreate(id,
                () => new DriveInfo(WmiUtility.GetDiskDriveObjectById(id)));

        internal static DriveInfo Get(ManagementObject source) =>
            GetOrCreate(source.GetConvertedProperty("DeviceId", Convert.ToString),
                () => new DriveInfo(source));

        public static DriveReadOnlyCollection LocalDrives
        {
            get
            {
                return new DriveReadOnlyCollection(WmiUtility.GetAllDiskDrives().Select(mobject => DriveReadOnlyCollection.Get(mobject)).ToList());
            }
        }
    }
}
