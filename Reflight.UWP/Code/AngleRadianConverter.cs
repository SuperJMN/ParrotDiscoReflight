using System;
using Windows.UI.Xaml.Data;

namespace ParrotDiscoReflight.Code
{
    public class AngleRadianConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = System.Convert.ToDouble(value);
            return v * 180D / Math.PI;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var v = System.Convert.ToDouble(value);
            return v * Math.PI / 180D;
        }
    }
}