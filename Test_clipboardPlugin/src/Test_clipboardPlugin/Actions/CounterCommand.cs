namespace Loupedeck.Test_clipboardPlugin
{
    using System;
    using Loupedeck.Midi;
    using Loupedeck;

    // This class implements an example command that counts button presses.

    public class CounterCommand : PluginDynamicCommand
    {
        ActionEditor instance = new ActionEditor();
        EverySecondTick newInst = new EverySecondTick(2);
        LoupedeckDeviceAttribute tests = new LoupedeckDeviceAttribute(Loupedeck.DeviceType.Loupedeck20);
        private readonly string myStr = "Ok";
        private Int32 _counter = 2;
        // Initializes the command class.
        public CounterCommand()
            : base(displayName: "Press Counter", description: "Counts button presses", groupName: "Commands")
        {
        }

        // This method is called when the user executes the command.
        protected override void RunCommand(String actionParameter)
        {
            int myNumber = 10;
            this.newInst.UpdateTicks(ref myNumber);
            this.Plugin.KeyboardApi.SendShortcut("c");
            this.Plugin.ExecuteGenericAction("SendShortcut", "d", 3);
            this.Plugin.ExecuteGenericAction("MidiNotePlay", "2",1);
            this.Plugin.ClientApplication.SendKeyboardShortcut(VirtualKeyCode.VolumeMute);
            this._counter++;
            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.
            PluginLog.Info($"Counter value is {this._counter}"); // Write the current counter value to the log file.
        }

        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"Press1 {this.myStr}  Counter{Environment.NewLine}{this._counter}";
    }
}
