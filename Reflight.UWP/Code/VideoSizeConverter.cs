using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ParrotDiscoReflight.Code
{
    public class VideoSizeConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(VideoSizeConverter), new PropertyMetadata(500D));

        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var convert = System.Convert.ToDouble(value);
            
            var final = convert > Minimum ? convert : Minimum;
            return final;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}