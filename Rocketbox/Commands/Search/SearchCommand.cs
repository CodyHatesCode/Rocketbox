using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rocketbox
{
    /// <summary>
    /// Command for launching search engine queries.
    /// </summary>
    internal class SearchCommand : IRbCommand
    {
        private RbSearchEngine _engine;

        public List<string> Keywords { get; } = new List<string>();

        // the search engine is determined by the Invoker and passed here
        internal SearchCommand(RbSearchEngine engine)
        {
            _engine = engine;
        }

        // will print the search engine's full name and the query to be sent
        public string GetResponse(string arguments)
        {
            string response = String.Format("{0}:   {1}", _engine.Name, arguments);
            return response;
        }

        // launches the query through a default browser
        public bool Execute(string arguments)
        {
            arguments = arguments.Replace("#", "%23"); // hashtags

            string url = _engine.Url_Prefix + arguments;

            Process.Start(url);

            return true;
        }

        public string GetIcon()
        {
            return _engine.Icon;
        }
    }
}
