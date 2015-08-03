using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiskMagic.DetectionLibrary;

namespace DiskMagic.UI
{
    class InfoViewModel
    {
        public DiskInfo CurrentDiskInfo { get; set; }

        public List<DiskInfo> Disks { get; private set; }
    }
}
