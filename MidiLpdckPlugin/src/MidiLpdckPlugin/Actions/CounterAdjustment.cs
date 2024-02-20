namespace Loupedeck.MidiLpdckPlugin
{
    using System;
    using System.Collections.Concurrent;
    using Melanchall.DryWetMidi.Core;
    using Melanchall.DryWetMidi.Multimedia;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class CounterAdjustment : PluginDynamicAdjustment
    {
        private MidiLpdckPlugin _plugin;
        private readonly ConcurrentDictionary<String, byte> _midiValues;
        private readonly OutputDevice outputDeviceNew;
        //private readonly VirtualDevice myVirtualDevice;
        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public CounterAdjustment()
            : base(displayName: "Midi rotation", description: "Change midi value on rotation", groupName: "Adjustments", hasReset: true)
        {
           
            this.MakeProfileAction("text;Midi note");
            this._midiValues = new ConcurrentDictionary<String, byte>();
            //this.myVirtualDevice = VirtualDevice.Create("MyDevice");
            this.outputDeviceNew = OutputDevice.GetByName("lpdckmidi");
        }

        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as MidiLpdckPlugin;
            return base.OnLoad();
        }


        private string giveMidiValue(string midiNote)
        {
            Byte valtosend = 63;
            if (this._midiValues.TryGetValue(midiNote, out var val))
            { valtosend = val; }
            else
            { valtosend = 63; }
            double scaledValue = valtosend / 12.7;

            if(valtosend == 127)
            {
                return scaledValue.ToString("0");
            }
            else
            { return scaledValue.ToString("0.0"); }
            
        }

        ~CounterAdjustment() {
            this.outputDeviceNew.Dispose();
            //this.myVirtualDevice.Dispose();
        }
        // This method is called when the adjustment is executed.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this._midiValues.TryGetValue(actionParameter, out var oldMidiVal))
            { }else
            {
                oldMidiVal = 63;
            }

            Byte midiNote= 63;
                    try
            {
                 midiNote = (byte)int.Parse(actionParameter);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input string is not in the correct format.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input string represents a number that is too large or too small for an int32.");
            }

            Byte newMidiVal;

            if (oldMidiVal + diff < 0)
            { newMidiVal = 0; }
            else if
             (oldMidiVal + diff > 127)
            { newMidiVal = 127; }
            else
            { newMidiVal = (byte)(oldMidiVal + diff); }

            if (this._midiValues.ContainsKey(actionParameter))
            { this._midiValues[actionParameter] = newMidiVal; }
            else
            { 
            this._midiValues.TryAdd(actionParameter, newMidiVal);
            }


            this.outputDeviceNew.SendEvent(
            new ControlChangeEvent(
                new Melanchall.DryWetMidi.Common.SevenBitNumber(midiNote),
                new Melanchall.DryWetMidi.Common.SevenBitNumber(newMidiVal)
                )
            );


            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }


        protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            using (BitmapBuilder bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                bitmapBuilder.DrawText($"{this.giveMidiValue(actionParameter)}", BitmapColor.White, 30);
                return bitmapBuilder.ToImage();
            }
        }


    }
}
