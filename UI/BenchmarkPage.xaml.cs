using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiskMagic.DetectionLibrary;

namespace DiskMagic.UI
{
    /// <summary>
    /// BenchmarkPage.xaml 的交互逻辑
    /// </summary>
    public partial class BenchmarkPage : Page
    {
        public BenchmarkPage(PartitionInfo partition = null)
        {
            InitializeComponent();
            model.Partition = partition; 
        }
    }
}
