using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocketbox
{
    internal static class RbGlobals
    {
        // Application information
        internal const string APPLICATION_NAME = "Rocketbox";
        internal const string RB_VERSION = "1.1.0";
        internal const string RB_BUILD_DATE = "2021-06-04";
        internal const string ICON_NAME = "rocket.ico";

        // Names of database files
        internal const string DB_FILE_NAME = "Rocketbox.db";
        internal const string DB_FAIL_FILE_NAME = "RocketboxFallback.db";

        // The text that appears in the tooltip when Rocketbox is launched
        internal const string LAUNCH_TOOLTIP_STRING = "Rocketbox is now active. Press Win + ~ to open it.";

        // Universal time/date format
        private static string _dateFmt = "dddd, MMMM d, yyyy  ―  h:mm tt";
        internal static string DateFormat { get { return _dateFmt; } }

        // Error messages
        internal const string DB_LOAD_ERR_MSG = "The Rocketbox database is missing or inaccessible. Certain features may not work properly. It is recommended that you replace this copy of Rocketbox.";
        internal const string ALREADY_RUNNING_ERR_MSG = "Rocketbox is already running, or another application has reserved its hotkey (Windows Key + ~). Please close existing instances of Rocketbox or any other application that may interfere with it before attempting to launch it again.";

        // Relative path to where assets are stored
        internal const string ASSET_DIR = @"icons\";

        // Names of other important files in the Rocketbox directory
        internal const string LICENSE_FILE_NAME = "LICENSE";
        internal const string THIRD_PARTY_LICENSE_FILE_NAME = "LICENSE-THIRD-PARTY";

        // Strings for certain commands
        internal const string DATE_CALC_ERROR_STRING = "Cannot compute date/time.";
        internal const string DATE_CALC_OUT_STRING = "Calculated date/time:   ";
    }
}
