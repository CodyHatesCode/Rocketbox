using System;

namespace Rocketbox
{
    /// <summary>
    /// Command for converting Unix time into the local date/time
    /// </summary>
    internal class FromUnixTimeCommand : IRbCommand
    {
        private long _epochValue;
        private string _dateString;

        public FromUnixTimeCommand() { }

        public bool Execute(string arguments)
        {
            if (_dateString != string.Empty)
            {
                System.Windows.Clipboard.SetText(_dateString);
            }

            return false;
        }

        public string GetResponse(string arguments)
        {
            _dateString = string.Empty;

            if (arguments.Trim() == string.Empty)
            {
                return "Convert from Unix time...";
            }

            if (!long.TryParse(arguments, out _epochValue))
            {
                return "Unable to parse Unix time.";
            }

            try
            {
                DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(_epochValue);
                _dateString = "Local time:   " + dt.ToLocalTime().ToString(RbGlobals.DateFormat);
            }
            catch (ArgumentOutOfRangeException)
            {
                _dateString = "Unable to parse Unix time.";
            }

            return _dateString;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
