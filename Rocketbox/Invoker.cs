using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocketbox
{
    /// <summary>
    /// Handles the determination of what actions to be taken with a user command
    /// </summary>
    internal static class Invoker
    {
        private static string _currentText;
        private static List<IRbCommand> _registeredCommands;
        private static IRbCommand _currentCmd;

        internal static bool ShutdownNow { get; set; }
        
        /// <summary>
        /// Keyword of the current text command
        /// </summary>
        private static string Keyword
        {
            get
            {
                return RbUtility.GetKeyword(_currentText).ToUpper();
            }
        }

        /// <summary>
        /// Parameters of the current text command
        /// </summary>
        private static string Parameters
        {
            get
            {
                return RbUtility.StripKeyword(_currentText);
            }
        }

        /// <summary>
        /// Registers the commands that can be executed by the Invoker
        /// </summary>
        internal static void RegisterCommands()
        {
            // SearchCommand, NullCommand, and ShellCommand are not "registered" conventionally, as they are special cases
            _registeredCommands = new List<IRbCommand>()
            {
                // Search
                new ClipboardSearchCommand(),
                new TranslateCommand(),

                // System
                new AboutDialogCommand(),
                new ExitCommand(),
                new InstallPackCommand(),
                new ListPackCommand(),
                new UninstallPackCommand(),

                // TimeDate
                new AddTimeCommand(),
                new FromUnixTimeCommand(),
                new SubtractTimeCommand(),
                new TimeCommand(),
                new TimeCompareCommand(),
                new ToUnixTimeCommand(),
                new UnixTimeCommand(),

                // Utility
                new CalculatorCommand(),
                new UnitConversionCommand()
            };
        }

        /// <summary>
        /// Parses text and chooses the appropriate command
        /// </summary>
        /// <param name="command">The text of the command to be parsed</param>
        internal static void Invoke(string command)
        {
            ShutdownNow = false;

            // default to a shell command
            _currentCmd = new ShellCommand(command);
            _currentText = command;

            // first, test for search engines
            var matchingSearchEngines = RbData.SearchEngines.Where(e => e.Aliases.Contains(Keyword));

            // if a search engine is found, change the command
            if (matchingSearchEngines.Count() != 0)
            {
                _currentCmd = new SearchCommand(matchingSearchEngines.First());
            }

            // if we did not find a search command, try the other commands
            if(_currentCmd.GetType() == typeof(ShellCommand))
            {
                var foundCommands = _registeredCommands.Where(c => c.Keywords.Contains(Keyword));
                if(foundCommands.Count() > 0)
                {
                    _currentCmd = foundCommands.First();
                }
            }
        }

        /// <summary>
        /// Retrieves the string to be indicated below the text box before a command is sent
        /// </summary>
        /// <returns>Text returned by the command</returns>
        internal static string GetResponse()
        {
            if(_currentText.Trim() == string.Empty)
            {
                return new NullCommand().GetResponse(_currentText);
            }

            return _currentCmd.GetResponse(Parameters);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>Whether the command was successful or not</returns>
        internal static bool Execute()
        {
            if(_currentText.Trim() == string.Empty)
            {
                return false;
            }

            return _currentCmd.Execute(Parameters);
        }

        /// <summary>
        /// Obtains the icon from the command
        /// </summary>
        /// <returns>The file name of the icon to be displayed</returns>
        internal static string GetIcon()
        {
            if(_currentText.Trim() == string.Empty)
            {
                return new NullCommand().GetIcon();
            }

            return _currentCmd.GetIcon();
        }
    }
}
