using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Drivelens.DetectionLibrary.Wmi;

namespace Drivelens.DetectionLibrary
{
    public abstract class WmiDeviceInfoObjectBase : IIdentifiable<string>
    {
        protected WmiDeviceInfoObjectBase(ManagementObject source)
        {
            this.RefreshPropertiesFromWmiObject(source);
        }

        public abstract void RefreshProperties();

        protected virtual void RefreshPropertiesFromWmiObject(ManagementObject source)
        {
            this.DeviceId = source.GetConvertedProperty("DeviceId", Convert.ToString);
        }

        public string DeviceId { get; protected set; }

        public string Identifier { get { return this.DeviceId; } }
    }
}
