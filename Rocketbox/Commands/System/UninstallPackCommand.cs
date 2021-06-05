using System;

namespace Rocketbox
{
    /// <summary>
    /// Command to remove a search engine pack
    /// </summary>
    internal class UninstallPackCommand : IRbCommand
    {
        private bool _packExists = false;
        private bool _uninstallSuccessful = false;

        public UninstallPackCommand() { }

        public bool Execute(string arguments)
        {
            if (_packExists)
            {
                RbData.UninstallSearchPack(arguments.Trim().ToLower());
                _uninstallSuccessful = true;
            }

            return false;
        }

        public string GetResponse(string arguments)
        {
            _packExists = false;
            _uninstallSuccessful = false;

            string output;

            if (_uninstallSuccessful)
            {
                output = "Successfully uninstalled \"" + arguments + "\".";
            }
            else if (arguments.Trim() == string.Empty || arguments.Contains(" "))
            {
                output = "Uninstall a search engine pack...";
            }
            else if (arguments.ToLower().Contains("default"))
            {
                output = "Cannot remove a default pack.";
            }
            else if (!RbData.Packages.Contains(arguments.Trim().ToLower()))
            {
                output = "Pack \"" + arguments + "\" is not installed.";
            }
            else
            {
                output = "Uninstall search engine pack:  " + arguments;
                _packExists = true;
            }

            return output;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
