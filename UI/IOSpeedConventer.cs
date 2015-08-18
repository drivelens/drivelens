using System;
using System.Globalization;
using System.Windows.Data;
using DiskMagic.BenchmarkLibrary;

namespace DiskMagic.UI
{
    [ValueConversion(typeof(IOSpeed), typeof(double))]
    class IOSpeedConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
