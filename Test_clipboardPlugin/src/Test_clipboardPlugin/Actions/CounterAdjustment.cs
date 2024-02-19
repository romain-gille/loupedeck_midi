namespace Loupedeck.Test_clipboardPlugin
{
    using System;

    //using Melanchall.DryWetMidi.Core;
    //using Melanchall.DryWetMidi.Multimedia;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class CounterAdjustment : PluginDynamicAdjustment
    {
        // This variable holds the current value of the counter.
        private Int32 _counter = 0;
        //OutputDevice outputDeviceNew;

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Tick Counter", description: "Counts rotation ticks", groupName: "Adjustments", hasReset: true)
        {
            //this.outputDeviceNew = OutputDevice.GetByName("midi1");

        }

         //~CounterAdjustment() { this.outputDeviceNew.Dispose(); }


        // This method is called when the adjustment is executed.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            //if(this._counter + diff<0)
            //{this._counter =0;} else if
            // (this._counter + diff > 127)
            //{ this._counter =127;} else
            //{ this._counter += (byte)diff; }
            this._counter += diff;
            // Increase or decrease the counter by the number of ticks.

            //this.outputDeviceNew.SendEvent(
            //new ControlChangeEvent(
            //    new Melanchall.DryWetMidi.Common.SevenBitNumber(1),
            //    new Melanchall.DryWetMidi.Common.SevenBitNumber(this._counter)
            //    )
            //);

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
