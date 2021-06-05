using System;

namespace Rocketbox
{
    /// <summary>
    /// Command for comparing the difference between now and another date
    /// </summary>
    internal class TimeCompareCommand : IRbCommand
    {
        public TimeCompareCommand() { }

        public string GetResponse(string arguments)
        {
            string output = "Time since/until...";
            bool error = false;

            DateTime dtToCompare = DateTime.Now;

            try
            {
                dtToCompare = DateTime.Parse(arguments);
            }
            catch { error = true; }

            TimeSpan diff;

            if (dtToCompare > DateTime.Now)
            {
                output = string.Format("Time until {0}:  ", dtToCompare.ToString(RbGlobals.DateFormat));
                diff = dtToCompare - DateTime.Now;
            }
            else
            {
                output = string.Format("Time since {0}:  ", dtToCompare.ToString(RbGlobals.DateFormat));
                diff = DateTime.Now - dtToCompare;
            }

            // to save space, only displaying units that have a non-zero value
            int years = 0;
            while (diff.Days >= 365)
            {
                diff = diff.Subtract(new TimeSpan(365, 0, 0, 0));
                years++;
            }
            if (years > 0)
            {
                output += string.Format(" {0} years", years);
            }

            if (diff.Days > 0)
            {
                output += string.Format(" {0} days", diff.Days);
            }

            if (diff.Hours > 0)
            {
                output += string.Format(" {0} hours", diff.Hours);
            }

            if (diff.Minutes > 0)
            {
                output += string.Format(" {0} minutes", diff.Minutes);
            }

            if (error) { output = "Time since/until:   Unable to parse date."; }

            return output;
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
