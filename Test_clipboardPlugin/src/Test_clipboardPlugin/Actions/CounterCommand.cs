namespace Loupedeck.Test_clipboardPlugin
{
    using System;
    using Loupedeck;
    //using Melanchall.DryWetMidi.Multimedia;
    //using Melanchall.DryWetMidi.Core;

    // This class implements an example command that counts button presses.

    public class CounterCommand : PluginDynamicCommand

    {
        private Int32 _counter = 0;

        // Initializes the command class.
        public CounterCommand()
            : base(displayName: "Press Counter", description: "Counts button presses", groupName: "Commands")
        {
        }

        // This method is called when the user executes the command.
        protected override void RunCommand(String actionParameter)
        {
            this._counter++;

            //using (var outputDevice = OutputDevice.GetByName("midi1"))
            //{
            //    outputDevice.SendEvent(
            //    new NoteOnEvent(
            //        new Melanchall.DryWetMidi.Common.SevenBitNumber(2),
            //        new Melanchall.DryWetMidi.Common.SevenBitNumber(127)
            //        )
            //    );
            //    outputDevice.SendEvent(
            //    new NoteOffEvent(
            //        new Melanchall.DryWetMidi.Common.SevenBitNumber(2),
            //        new Melanchall.DryWetMidi.Common.SevenBitNumber(127)
            //        )
            //    );
            //}
            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.
        }

        // This method is called when Loupedeck needs to show the command on the console or the UI. 
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"  Counter{Environment.NewLine}{this._counter}";
    }
}
