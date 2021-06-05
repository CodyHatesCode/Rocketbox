using System;

namespace Rocketbox
{
    internal class AboutDialogCommand : IRbCommand
    {
        public AboutDialogCommand() { }

        public bool Execute(string arguments)
        {
            new AboutWindow().Show();
            return true;
        }

        public string GetResponse(string arguments)
        {
            return "About Rocketbox...";
        }

        public string GetIcon()
        {
            return RbGlobals.ICON_NAME;
        }
    }
}
