using System;
using System.Collections.Generic;

namespace Rocketbox
{
    internal class ExitCommand : IRbCommand
    {
        public List<string> Keywords { get; } = new List<string> { "EXIT" };

        public ExitCommand() { }

        public bool Execute(string arguments)
        {
            Invoker.ShutdownNow = true;
            return true;
        }

        public string GetResponse(string arguments)
        {
            return "Shut down Rocketbox...";
        }

        public string GetIcon()
        {
            return RbGlobals.ICON_NAME;
        }
    }
}
