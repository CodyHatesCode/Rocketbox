using System;
using System.Collections.Generic;

namespace Rocketbox
{
    /// <summary>
    /// Dummy command for blank or whitespace-only input.
    /// </summary>
    internal class NullCommand : IRbCommand
    {
        public List<string> Keywords { get; } = new List<string>();

        internal NullCommand() { }

        public string GetResponse(string arguments)
        {
            return "Invalid command.";
        }

        public bool Execute(string arguments)
        {
            // do nothing
            return false;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
