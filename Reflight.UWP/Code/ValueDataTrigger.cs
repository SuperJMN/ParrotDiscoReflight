using Windows.UI.Xaml;

namespace ParrotDiscoReflight.Code
{
    public class ValueDataTrigger : StateTriggerBase
    {
        private static void TriggerStateCheck(DependencyObject target, object dataValue, object triggerValue)
        {
            var trigger = target as ValueDataTrigger;
            if (trigger == null) return;
            trigger.SetActive(triggerValue == dataValue);
        }

        #region DataValue

        public static object GetDataValue(DependencyObject obj)
        {
            return obj.GetValue(DataValueProperty);
        }

        public static void SetDataValue(DependencyObject obj, object value)
        {
            obj.SetValue(DataValueProperty, value);
        }

        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.RegisterAttached("DataValue", typeof(object),
                typeof(ValueDataTrigger), new PropertyMetadata(null, DataValueChanged));

        private static void DataValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var triggerValue = target.GetValue(TriggerValueProperty);
            TriggerStateCheck(target, e.NewValue, triggerValue);
        }

        #endregion

        #region TriggerValue

        public static object GetTriggerValue(DependencyObject obj)
        {
            return obj.GetValue(TriggerValueProperty);
        }

        public static void SetTriggerValue(DependencyObject obj, object value)
        {
            obj.SetValue(TriggerValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for TriggerValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TriggerValueProperty =
            DependencyProperty.RegisterAttached("TriggerValue", typeof(object),
                typeof(ValueDataTrigger), new PropertyMetadata(false, TriggerValueChanged));

        private static void TriggerValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var dataValue = target.GetValue(DataValueProperty);
            TriggerStateCheck(target, dataValue, e.NewValue);
        }

        #endregion
    }
}