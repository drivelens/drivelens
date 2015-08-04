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
        public ObservableCollection<BenchmarkTestViewModel> BenchmarkProviders { get; } = new ObservableCollection<BenchmarkTestViewModel>(
        new IBenchmarkProvider[] {
            BenchmarkTestProviders.SequenceBenchmarkProvider,
            BenchmarkTestProviders.Random4KBenchmarkProvider,
            BenchmarkTestProviders.Random512KBenchmarkProvider,
            BenchmarkTestProviders.Random4K64ThreadRandomBenchmarkProvider,
        }.Select(s => new BenchmarkTestViewModel(s)));

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

        BenchmarkFlags flags;
    }

    class BenchmarkTestViewModel : ViewModelBase
    {
        bool isSelected = true;
        
        public BenchmarkTestViewModel(IBenchmarkProvider provider)
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

        public ObservableCollection<Tuple<BenchmarkType, IOSpeed?>> Results { get; } = 
            new ObservableCollection<Tuple<BenchmarkType, IOSpeed?>>(
                new[] { BenchmarkType.Read, BenchmarkType.Write }
                .Select(type => Tuple.Create<BenchmarkType, IOSpeed?>(type, null)));

        public void UpdateResult(PartitionInfo partition,BenchmarkFlags flags,CancellationToken cancellationToken)
        {
            var result = Results
                         .Select(item => Tuple.Create(item.Item1, (IOSpeed?)Provider.GetTestResult(partition, item.Item1, flags, cancellationToken)));
            Results.Clear();
            result.ForEach(a => Results.Add(a));
        }
    }
}
