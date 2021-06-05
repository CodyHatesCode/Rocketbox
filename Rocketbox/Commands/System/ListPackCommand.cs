using System;
using System.Collections.Generic;

namespace Rocketbox
{
    /// <summary>
    /// Command to dump list of installed search engine packs
    /// </summary>
    internal class ListPackCommand : IRbCommand
    {
        public List<string> Keywords { get; } = new List<string> { "PACKS" };

        public ListPackCommand() { }

        public bool Execute(string arguments)
        {
            RbData.DumpInstalledPacks();
            new ShellCommand("notepad.exe RocketboxPackages.txt").Execute("");
            return true;
        }

        public string GetResponse(string arguments)
        {
            return "List installed search engine packs...";
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }

}
