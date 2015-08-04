using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiskMagic.DetectionLibrary;

namespace DiskMagic.UI.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        PartitionInfo currentPartitionInfo;

        public PartitionInfo CurrentPartitionInfo
        {
            get { return currentPartitionInfo; }
            set
            {
                currentPartitionInfo = value;
                NotifyPropertyChanged();
            }
        }

        public PartitionInfo[] Partitions { get; } = DriveDetector.GetPartitions();
    }
}
