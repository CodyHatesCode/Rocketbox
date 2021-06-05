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

        // Error message for failed database load
        internal const string DB_LOAD_ERR_MSG = "The Rocketbox database is missing or inaccessible. Certain features may not work properly. It is recommended that you replace this copy of Rocketbox.";

        // Relative path to where assets are stored
        internal const string ASSET_DIR = @"icons\";

        // Names of other important files in the Rocketbox directory
        internal const string LICENSE_FILE_NAME = "LICENSE";
        internal const string THIRD_PARTY_LICENSE_FILE_NAME = "LICENSE-THIRD-PARTY";
    }
}
