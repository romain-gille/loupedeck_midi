namespace Loupedeck.Test_clipboardPlugin
{
    using System;

    public class CounterAdjustment : PluginDynamicAdjustment
    {
        Int32 _counter = 0;
        // This variable holds the current value of the counter.

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Tick Counter", description: "Counts rotation ticks", groupName: "Adjustments", hasReset: true)
        {

        }

        // This method is called when the adjustment is executed.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {

            this._counter += diff;

            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            this._counter = 0; // Reset the counter.
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) => this._counter.ToString();
    }
}
