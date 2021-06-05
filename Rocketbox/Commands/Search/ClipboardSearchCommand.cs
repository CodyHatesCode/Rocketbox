using System;
using System.Linq;

namespace Rocketbox
{
    /// <summary>
    /// Command to search using text from the clipboard
    /// </summary>
    internal class ClipboardSearchCommand : IRbCommand
    {
        // chain a SearchCommand
        SearchCommand _command;

        internal ClipboardSearchCommand() { }

        public bool Execute(string arguments)
        {
            if (arguments.Trim() == string.Empty || _command == null)
            {
                return false;
            }

            return _command.Execute(System.Windows.Clipboard.GetText());
        }

        public string GetIcon()
        {
            if (_command != null)
            {
                return _command.GetIcon();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetResponse(string arguments)
        {
            if (arguments.Trim() == string.Empty)
            {
                return "Search using clipboard...";
            }

            var matchingSearchEngines = RbData.SearchEngines.Where(e => e.Aliases.Contains(arguments.ToUpper()));

            if (matchingSearchEngines.Count() == 0)
            {
                return "Clipboard search:  Search engine not found.";
            }

            _command = new SearchCommand(matchingSearchEngines.First());
            return _command.GetResponse(System.Windows.Clipboard.GetText());
        }
    }
}
