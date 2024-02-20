namespace Loupedeck.Test_clipboardPlugin
{
    using System;
    using System.Collections.Concurrent;

    using Loupedeck;


    // This class implements an example command that counts button presses.

    public class CounterCommand : PluginDynamicCommand

    {
        private Test_clipboardPlugin _plugin;
        private readonly ConcurrentDictionary<String, String> _textvalues;

        // Initializes the command class.
        public CounterCommand()
            : base(displayName: "Show text", description: "Simply show text", groupName: "Commands")
        {
            this.MakeProfileAction("text;text to show");
            this._textvalues = new ConcurrentDictionary<String, String>();


        }

        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as Test_clipboardPlugin;
            return base.OnLoad();
        }
        private string giveTextValue(string textVal)
        {
            if (this._textvalues.TryGetValue(textVal, out var text))
            { }
            else
            { text = ""; }
            return text;

        }


        // This method is called when the user executes the command.
        protected override void RunCommand(String actionParameter)
        {
            if (this._textvalues.ContainsKey(actionParameter))
            { this._textvalues[actionParameter] = actionParameter; }
            else
            {
                this._textvalues.TryAdd(actionParameter, actionParameter);
            }


            this.ActionImageChanged(); // Notify the Loupedeck service that the command display name and/or image has changed.
        }

        protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            using (BitmapBuilder bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                bitmapBuilder.DrawText($"{this.giveTextValue(actionParameter)}", BitmapColor.White, 22);
                return bitmapBuilder.ToImage();
            }
        }
    }
}
