using System;
using System.Globalization;
using System.Windows.Data;
using DiskMagic.BenchmarkLibrary;

namespace DiskMagic.UI
{
    [ValueConversion(typeof(IOSpeed), typeof(string))]
    class IOSpeedConventer : IMultiValueConverter
    {
        public object Convert(object[] untypedValues, Type targetType, object untypedParameter, CultureInfo culture)
        {
            var value = (IOSpeed?)untypedValues[0];
            var parameter = (string)untypedValues[1];
            switch (parameter)
            {
                case "IO":
                    return value?.IOPerSecond.ToString();
                //case "MB":
                default:
                    return value?.MegabytePerSecond.ToString();
            }
        }

        public object[] ConvertBack(object untypedValue, Type[] targetType, object untypedParameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

    [ValueConversion(typeof(BenchmarkFlags),typeof(bool))]
    class BenchmarkFlagsConventer : IValueConverter
    {
        public object Convert(object untypedValue, Type targetType, object parameter, CultureInfo culture)
        {
            var value = (BenchmarkFlags)untypedValue;
            return value.HasFlag(BenchmarkFlags.Compressible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? BenchmarkFlags.Compressible : BenchmarkFlags.None;
    }

    [ValueConversion(typeof(bool),typeof(string))]
    class ConventBoolToMBOrIO : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? "MB" : "IO";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string)value == "MB";
    }
}