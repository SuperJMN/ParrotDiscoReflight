using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SuppaFlight.UWP.Code
{
    public class BindableStringFormatConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(BindableStringFormatConverter), new PropertyMetadata(default(string)));

        public string StringFormat
        {
            get { return (string) GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(StringFormat, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}