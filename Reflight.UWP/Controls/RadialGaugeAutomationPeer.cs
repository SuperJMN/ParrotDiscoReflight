using System.Collections.Generic;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace ParrotDiscoReflight.Controls
{
    public class RadialGaugeAutomationPeer :
        FrameworkElementAutomationPeer,
        IRangeValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGaugeAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner element to create for.</param>
        public RadialGaugeAutomationPeer(AnimatedRadialGauge owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        public bool IsReadOnly => !((AnimatedRadialGauge)Owner).IsInteractive;

        /// <inheritdoc/>
        public double LargeChange => ((AnimatedRadialGauge)Owner).StepSize;

        /// <inheritdoc/>
        public double Maximum => ((AnimatedRadialGauge)Owner).Maximum;

        /// <inheritdoc/>
        public double Minimum => ((AnimatedRadialGauge)Owner).Minimum;

        /// <inheritdoc/>
        public double SmallChange => ((AnimatedRadialGauge)Owner).StepSize;

        /// <inheritdoc/>
        public double Value => ((AnimatedRadialGauge)Owner).Value;

        /// <inheritdoc/>
        public void SetValue(double value)
        {
            ((AnimatedRadialGauge)Owner).Value = value;
        }

        /// <inheritdoc/>
        protected override IList<AutomationPeer> GetChildrenCore()
        {
            return null;
        }

        /// <inheritdoc/>
        protected override string GetNameCore()
        {
            var gauge = (AnimatedRadialGauge)Owner;
            return "radial gauge. " + (string.IsNullOrWhiteSpace(gauge.Unit) ? "no unit specified. " : "unit " + gauge.Unit + ". ");
        }

        /// <inheritdoc/>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.RangeValue)
            {
                // Expose RangeValue properties.
                return this;
            }

            return base.GetPatternCore(patternInterface);
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }       
    }
}