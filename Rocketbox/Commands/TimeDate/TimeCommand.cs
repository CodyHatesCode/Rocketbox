using System;

namespace Rocketbox
{
    /// <summary>
    /// Command for simply displaying a date/time
    /// </summary>
    internal class TimeCommand : IRbCommand
    {
        internal TimeCommand() { }

        public string GetResponse(string arguments)
        {
            string timeStr = string.Format("Current date/time:   {0}", DateTime.Now.ToString(RbGlobals.DateFormat));

            return timeStr;
        }

        public bool Execute(string arguments)
        {
            return false;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
