using System;

namespace Rocketbox
{
    /// <summary>
    /// Command for getting the current Unix timestamp
    /// </summary>
    internal class UnixTimeCommand : IRbCommand
    {
        private string _unixTimeString;

        public UnixTimeCommand() { }

        public bool Execute(string arguments)
        {
            System.Windows.Clipboard.SetText(_unixTimeString);
            return false;
        }

        public string GetResponse(string arguments)
        {
            _unixTimeString = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            return "Current Unix timestamp:  " + _unixTimeString;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
