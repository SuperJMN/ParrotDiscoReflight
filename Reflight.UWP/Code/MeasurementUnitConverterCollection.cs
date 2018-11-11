using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class MeasurementUnitConverterCollection : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var unit = MeasurementUnit;
            if (unit == null)
            {
                return null;
            }

            var enumerable = (IEnumerable) value;
            var resultingCollection = enumerable.Cast<double>();

            return resultingCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty MeasurementUnitProperty = DependencyProperty.Register(
            "MeasurementUnit", typeof(IMeasurementUnit), typeof(MeasurementUnitConverterCollection), new PropertyMetadata(default(IMeasurementUnit)));

        public IMeasurementUnit MeasurementUnit
        {
            get { return (IMeasurementUnit) GetValue(MeasurementUnitProperty); }
            set { SetValue(MeasurementUnitProperty, value); }
        }
    }
}