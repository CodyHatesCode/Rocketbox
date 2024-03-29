﻿using System;
using System.Collections.Generic;

namespace Rocketbox
{
    /// <summary>
    /// Command for simply displaying a date/time
    /// </summary>
    internal class TimeCommand : IRbCommand
    {
        public List<string> Keywords { get; } = new List<string> { "TIME", "T" };

        internal TimeCommand() { }

        public string GetResponse(string arguments)
        {
            string timeStr = string.Format("Current date/time:   {0}", DateTime.Now.ToString(RbGlobals.CALC_DATE_FORMAT));

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
