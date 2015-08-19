using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DiskMagic.BenchmarkLibrary;
using DiskMagic.DetectionLibrary;
using System.Collections.ObjectModel;

namespace DiskMagic.UI.ViewModels
{
    class BenchmarkViewModel : ViewModelBase
    {
        public ObservableCollection<BenchmarkTestModel> BenchmarkProviders { get; } = new ObservableCollection<BenchmarkTestModel>(
        new IBenchmarkProvider[] {
            BenchmarkTestProviders.SequenceBenchmarkProvider,
            BenchmarkTestProviders.Random4KBenchmarkProvider,
            BenchmarkTestProviders.Random512KBenchmarkProvider,
            BenchmarkTestProviders.Random4K64ThreadRandomBenchmarkProvider,
        }.Select(s => new BenchmarkTestModel(s)));

        public PartitionInfo Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                NotifyPropertyChanged();
            }
        }

        public BenchmarkFlags Flags
        {
            get { return flags; }
            set
            {
                flags = value;
                NotifyPropertyChanged();
            }
        }

        PartitionInfo partition;

        BenchmarkFlags flags = BenchmarkFlags.Compressible;
        
        public void StartMenchmark(CancellationToken cancellationToken)
        {
            BenchmarkProviders.ForEach(item => item.UpdateResult(Partition, Flags, cancellationToken));
        }
    }

    class BenchmarkTestModel : ViewModelBase
    {
        bool isSelected = true;
        
        public BenchmarkTestModel(IBenchmarkProvider provider)
        {
            Provider = provider;
        }

        public IBenchmarkProvider Provider { get; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<TypeSpeedPair> Results { get; } = 
            new ObservableCollection<TypeSpeedPair>(
                new[] { BenchmarkType.Read, BenchmarkType.Write }
                .Select(type => new TypeSpeedPair(type, null)));

        public void UpdateResult(PartitionInfo partition,BenchmarkFlags flags,CancellationToken cancellationToken)
        {
            if (IsSelected)
                for (int i = 0; i < Results.Count; i++)
                {
                    var item = Results[i];
                    Results[i] = new TypeSpeedPair(item.BenchmarkType, Provider.GetTestResult(partition, item.BenchmarkType, flags, cancellationToken));
                }
        }
    }

    class TypeSpeedPair
    {
        public TypeSpeedPair(BenchmarkType type, IOSpeed? speed)
        {
            BenchmarkType = type;
            Speed = speed;
        }

        public BenchmarkType BenchmarkType { get; set; }

        public IOSpeed? Speed { get; set; }
    }
}
