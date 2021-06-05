using System;
using System.Collections.Generic;

namespace Rocketbox
{
    /// <summary>
    /// Command to convert a date/time into Unix time
    /// </summary>
    internal class ToUnixTimeCommand : IRbCommand
    {
        private DateTime _dt;
        private long _epochValue;
        private bool _goodConversion;

        public List<string> Keywords { get; } = new List<string> { "UTO" };

        public bool Execute(string arguments)
        {
            if (_goodConversion)
            {
                System.Windows.Clipboard.SetText(_epochValue.ToString());
            }
            return false;
        }

        public string GetResponse(string arguments)
        {
            _goodConversion = false;

            if (arguments.Trim() == string.Empty)
            {
                _goodConversion = false;
                return "Convert to Unix time...";
            }

            try
            {
                _dt = DateTime.Parse(arguments);
            }
            catch { return "Unable to parse date/time."; }

            _epochValue = ((DateTimeOffset)_dt).ToUnixTimeSeconds();

            _goodConversion = true;
            return "Unix time:  " + _epochValue;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
