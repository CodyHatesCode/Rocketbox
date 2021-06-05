using System;
using System.Collections.Generic;

namespace Rocketbox
{
    /// <summary>
    /// Defines the structure of a command to be used with the Invoker.
    /// </summary>
    internal interface IRbCommand
    {
        /// <summary>
        /// The keywords which can trigger this command
        /// </summary>
        List<string> Keywords { get; }

        /// <summary>
        /// Gets the response text of the command
        /// </summary>
        /// <param name="arguments">The arguments of the command</param>
        /// <returns>The text to be displayed underneath the input box</returns>
        string GetResponse(string arguments);

        /// <summary>
        /// Executes the indicated command
        /// </summary>
        /// <param name="arguments">The arguments of the command</param>
        /// <returns>Whether the command executed successfully</returns>
        bool Execute(string arguments);

        /// <summary>
        /// Gets the icon for this command
        /// </summary>
        /// <returns>The file name of the icon</returns>
        string GetIcon();
    }
}
