using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ParrotDiscoReflight.Code
{
    public class MeasurementUnitConverterToFormattedString : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var unit = MeasurementUnit;
            if (unit == null)
            {
                return null;
            }

            var d = System.Convert.ToDouble(value);

            return string.Format(unit.StringFormat, unit.Convert(d));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty MeasurementUnitProperty = DependencyProperty.Register(
            "MeasurementUnit", typeof(IMeasurementUnit), typeof(MeasurementUnitConverterToFormattedString), new PropertyMetadata(default(IMeasurementUnit)));

        public IMeasurementUnit MeasurementUnit
        {
            get { return (IMeasurementUnit) GetValue(MeasurementUnitProperty); }
            set { SetValue(MeasurementUnitProperty, value); }
        }
    }
}