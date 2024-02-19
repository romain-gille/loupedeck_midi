namespace Loupedeck.MidiLpdckPlugin
{
    using System;
    using System.Collections.Concurrent;
    using Melanchall.DryWetMidi.Core;
    using Melanchall.DryWetMidi.Multimedia;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class CounterAdjustment : PluginDynamicAdjustment
    {

        //private MidiLpdckPlugin _plugin;
        // This variable holds the current value of the counter.
        private Byte _counter = 0;
        //private readonly ConcurrentDictionary<String, dynamic> _midiValues;
        private readonly OutputDevice outputDeviceNew;
        //private readonly VirtualDevice myVirtualDevice;
        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Midi rotation", description: "Change midi value on rotation", groupName: "Adjustments", hasReset: true)
        {
            this.MakeProfileAction("text;Param1");
            //this._midiValues = new ConcurrentDictionary<String, dynamic>();
            //this.myVirtualDevice = VirtualDevice.Create("MyDevice");
            this.outputDeviceNew = OutputDevice.GetByName("lpdckmidi");
        }
        //protected override Boolean OnLoad()
        //{
        //    this._plugin = base.Plugin as MidiLpdckPlugin;
        //    this._plugin.AdjustmentValueChanged += (sender, e) => this.onAdjustment(sender, e);
        //    return base.OnLoad();
        //}

        //public void onAdjustment(object sender, ActionStateChangedEventArgs e)
        //{
        //    this.ApplyAdjustment("",1);
        //}

        ~CounterAdjustment() {
            this.outputDeviceNew.Dispose();
            //this.myVirtualDevice.Dispose();
        }
        // This method is called when the adjustment is executed.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            Byte midiVal = 3;

            try
            {
                midiVal = (byte)int.Parse(actionParameter);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input string is not in the correct format.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input string represents a number that is too large or too small for an int32.");
            }



            if (this._counter + diff < 0)
            { this._counter = 0; }
            else if
             (this._counter + diff > 127)
            { this._counter = 127; }
            else
            { this._counter += (byte)diff; }

            this.outputDeviceNew.SendEvent(
            new ControlChangeEvent(
                new Melanchall.DryWetMidi.Common.SevenBitNumber(midiVal),
                new Melanchall.DryWetMidi.Common.SevenBitNumber(this._counter)
                )
            );
            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.

            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            this._counter = 0; // Reset the counter.
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
    $"{this._counter}";

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) => this._counter.ToString();
    }
}
