using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rocketbox
{
    /// <summary>
    /// Command to launch an application as if it were from a normal command line.
    /// </summary>
    internal class ShellCommand : IRbCommand
    {
        private string _command;

        public List<string> Keywords { get; } = new List<string>();

        internal ShellCommand(string command)
        {
            _command = command;
        }

        // parameter can be null
        public string GetResponse(string arguments)
        {
            return string.Format("Run: {0}", _command);
        }

        // parameter can be null
        public bool Execute(string arguments)
        {
            // the entire command is passed into the constructor
            // we need to differentiate between the process and the arguments because System.Diagnostics said so
            string process = RbUtility.GetKeyword(_command);
            string args = RbUtility.StripKeyword(_command);

            ProcessStartInfo info = new ProcessStartInfo(@process);
            info.Arguments = args;
            try
            {
                Process.Start(info);
            }
            catch (Exception)
            {
                // if launch fails, just do nothing
                return false;
            }

            return true;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
