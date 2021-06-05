using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocketbox
{
    /// <summary>
    /// Command to add time to the current time/date
    /// </summary>
    internal class AddTimeCommand : IRbCommand
    {
        public List<string> Keywords { get; } = new List<string> { "T+" };

        public string GetResponse(string arguments)
        {
            int beforeCEYear = -1;
            DateTime calcDate;

            string outString = "Add date/time...";

            if(outString.Trim() != string.Empty)
            {
                if(!RbUtility.AddOrSubtractTime(arguments, RbTimeDiffMode.Add, out calcDate, out beforeCEYear))
                {
                    outString = RbGlobals.DATE_CALC_ERROR_STRING;
                }
                else
                {
                    outString = string.Format(RbGlobals.DATE_CALC_OUT_STRING + "{0}", calcDate.ToString(RbGlobals.DateFormat));
                }
            }

            return outString;
        }

        public bool Execute(string arguments)
        {
            return false;
        }

        public string GetIcon()
        {
            return "calculator.png";
        }
    }
}
