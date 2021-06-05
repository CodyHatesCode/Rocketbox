using System;
using System.Text.RegularExpressions;

namespace Rocketbox
{
    /// <summary>
    /// Command for finding differences from the current time/date
    /// </summary>
    internal class TimeDiffCommand : IRbCommand
    {
        private RbTimeDiffMode _mode;

        internal TimeDiffCommand(RbTimeDiffMode mode)
        {
            _mode = mode;
        }

        public string GetResponse(string arguments)
        {
            bool error = false;
            bool beforeCE = false;
            int beforeCEYear = 0;
            string errorString = "Cannot compute date/time.";

            if (arguments.Trim() == string.Empty) { errorString = "Add/subtract date/time..."; }

            string[] parts = arguments.ToUpper().Split(' ');

            DateTime calcDate = DateTime.Now;

            // goes through each part of the arguments to decipher the units/amount of time specified
            foreach (string s in parts)
            {
                int diff = 0;
                try
                {
                    diff = int.Parse(Regex.Match(s, @"\d+").Value);
                }
                catch { error = true; }

                // whether we're adding to or subtracting from the current date/time
                if (_mode == RbTimeDiffMode.Subtract)
                {
                    diff = -diff;
                }

                if (s.EndsWith("MI") || s.EndsWith("MIN") || s.EndsWith("MINS") || s.EndsWith("MINUTE") || s.EndsWith("MINUTES"))
                {
                    calcDate = calcDate.AddMinutes(diff);
                }
                else if (s.EndsWith("H") || s.EndsWith("HR") || s.EndsWith("HRS") || s.EndsWith("HOUR") || s.EndsWith("HOURS"))
                {
                    calcDate = calcDate.AddHours(diff);
                }
                else if (s.EndsWith("D") || s.EndsWith("DAY") || s.EndsWith("DAYS"))
                {
                    calcDate = calcDate.AddDays(diff);
                }
                else if (s.EndsWith("MO") || s.EndsWith("MONTH") || s.EndsWith("MONTHS"))
                {
                    calcDate = calcDate.AddMonths(diff);
                }
                else if (s.EndsWith("Y") || s.EndsWith("YR") || s.EndsWith("YRS") || s.EndsWith("YEAR") || s.EndsWith("YEARS"))
                {
                    try
                    {
                        calcDate = calcDate.AddYears(diff);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (_mode == RbTimeDiffMode.Subtract)
                        {
                            beforeCE = true;
                            beforeCEYear = -(DateTime.Now.Year + diff);
                        }
                    }
                }
                else
                {
                    error = true;
                }
            }

            if (error)
            {
                return errorString;
            }
            else
            {
                if (!beforeCE)
                {
                    return string.Format("Calculated date/time:   {0}", calcDate.ToString(RbGlobals.DateFormat));
                }
                else
                {
                    return string.Format("Calculated date/time:   {0}, {1} BCE  ―  {2}", calcDate.ToString("MMMM d"), beforeCEYear, calcDate.ToString("h:mm tt"));
                }
            }
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
