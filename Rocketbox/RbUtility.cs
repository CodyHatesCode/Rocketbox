using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Rocketbox
{
    internal static class RbUtility
    {
        /// <summary>
        /// Gets the keyword from a command string
        /// </summary>
        /// <param name="input">The command string</param>
        /// <returns>The first word</returns>
        internal static string GetKeyword(string input)
        {
            string keyword = input.Split(' ')[0].ToUpper();

            return keyword;
        }

        /// <summary>
        /// Removes the keyword from a command string
        /// </summary>
        /// <param name="input">The command string</param>
        /// <returns>The string with the first word removed</returns>
        internal static string StripKeyword(string input)
        {
            string[] commandParts = input.Split(' ');
            commandParts[0] = string.Empty;
            return string.Join(" ", commandParts).Trim();
        }

        /// <summary>
        /// Checks if an icon exists in the icons directory
        /// </summary>
        /// <param name="icon">The icon file name to be checked</param>
        /// <returns></returns>
        internal static bool IconExists(string icon)
        {
            if(File.Exists(Environment.CurrentDirectory + @"\" + RbGlobals.ASSET_DIR + icon))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an .rbx custom pack record is valid
        /// </summary>
        /// <param name="line">The line to validate</param>
        /// <returns></returns>
        internal static bool ValidPackLine(string line)
        {
            if(!line.Contains(";;"))
            {
                return false;
            }

            string[] fields = line.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

            if(fields.Length != 3)
            {
                return false;
            }

            return true;
        }

        internal static bool AddOrSubtractTime(string timeString, RbTimeDiffMode diffMode, out DateTime calcDate, out int beforeCEYear)
        {
            beforeCEYear = -1;

            string[] parts = timeString.ToUpper().Split(' ');

            calcDate = DateTime.Now;

            foreach (string s in parts)
            {
                int diff;

                try
                {
                    diff = int.Parse(Regex.Match(s, @"\d+").Value);
                }
                catch { return false; }

                // whether we're adding to or subtracting from the current date/time
                if(diffMode == RbTimeDiffMode.Subtract)
                {
                    diff = -diff;
                }

                if (s.EndsWith("S") || s.EndsWith("SEC") || s.EndsWith("SECOND") || s.EndsWith("SECONDS"))
                {
                    calcDate = calcDate.AddSeconds(diff);
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
                        if (diffMode == RbTimeDiffMode.Subtract)
                        {
                            beforeCEYear = -(DateTime.Now.Year + diff);
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
