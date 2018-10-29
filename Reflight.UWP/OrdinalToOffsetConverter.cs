using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ParrotDiscoReflight
{
    public class OrdinalToOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double multiplier;
            if (parameter is string s)
            {
                multiplier = double.Parse(s);
            }
            else
            {
                multiplier = System.Convert.ToDouble(value);
            }

            var number = System.Convert.ToDouble(value);
            var offset = number * multiplier;
            return new Thickness(offset, offset, -offset, -offset);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}