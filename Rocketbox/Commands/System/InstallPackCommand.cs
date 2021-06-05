using System;

namespace Rocketbox
{
    /// <summary>
    /// Command to install a search engine pack
    /// </summary>
    internal class InstallPackCommand : IRbCommand
    {
        private bool _goodFile = false;

        private bool _installAttempted = false;
        private bool _installSuccessful = false;

        public InstallPackCommand() { }

        public bool Execute(string arguments)
        {
            if (_goodFile)
            {
                _installAttempted = true;

                if (RbData.InstallSearchPack(arguments))
                {
                    _installSuccessful = true;
                }
                else
                {
                    _installSuccessful = false;
                }
            }

            return false;
        }

        public string GetResponse(string arguments)
        {
            _goodFile = false;

            string output;

            if (_installAttempted)
            {
                if (_installSuccessful)
                {
                    output = "Successfully installed \"" + arguments + "\".";
                }
                else
                {
                    output = "Was not able to install \"" + arguments + "\". Please verify that the format is correct.";
                }

                _installAttempted = false;
            }
            else if (RbData.Packages.Contains(arguments.ToLower()))
            {
                output = "Pack \"" + arguments + "\" is already installed.";
            }
            else if (arguments.Trim() == string.Empty || arguments.Contains(" "))
            {
                output = "Install search engine pack...";
            }
            else if (arguments.ToLower().Contains("default"))
            {
                output = "Cannot add default packs.";
            }
            else if (System.IO.File.Exists(arguments + ".rbx"))
            {
                output = "Install search engine pack:  " + arguments;
                _goodFile = true;
            }
            else
            {
                output = "Search engine pack not found.";
                _goodFile = false;
            }

            return output;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
