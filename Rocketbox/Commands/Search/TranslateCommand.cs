using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rocketbox
{
    /// <summary>
    /// Command for launching Google Translate
    /// </summary>
    internal class TranslateCommand : IRbCommand
    {
        private RbTranslateLanguage _fromLang;
        private RbTranslateLanguage _toLang;
        private string _textToTranslate;

        internal TranslateCommand()
        {
            _fromLang = null;
            _toLang = null;
            _textToTranslate = "";
        }

        /// <summary>
        /// Determines the languages and then puts them into the command
        /// </summary>
        /// <param name="arguments">The language string</param>
        private void Decode(string arguments)
        {
            List<string> parts = arguments.Split(' ').ToList();

            if (parts.Count > 0)
            {
                var matchingLangs = from lang in RbData.TranslateLanguages
                                    where lang.Keywords.Contains(parts[0].ToUpper())
                                    select lang;
                if (matchingLangs.Count() > 0) { _fromLang = matchingLangs.First(); }
            }

            if (parts.Count > 1)
            {
                var matchingLangs = from lang in RbData.TranslateLanguages
                                    where lang.Keywords.Contains(parts[1].ToUpper())
                                    select lang;
                if (matchingLangs.Count() > 0) { _toLang = matchingLangs.First(); }
            }

            // finds the string to translate (starts at index 2 of the array)
            if (parts.Count > 2)
            {
                parts.RemoveRange(0, 2);
                _textToTranslate = string.Join(" ", parts);
            }

        }

        public string GetResponse(string arguments)
        {
            Decode(arguments);

            string nameA = "Unknown", nameB = "Unknown";

            if (_fromLang != null) { nameA = _fromLang.Name; }
            if (_toLang != null) { nameB = _toLang.Name; }

            return String.Format("Translate {0} to {1}: \"{2}\"", nameA, nameB, _textToTranslate);
        }

        public bool Execute(string arguments)
        {
            Decode(arguments);

            if (_fromLang == null || _toLang == null)
            {
                return false;
            }
            else
            {
                string translateUrl = string.Format("https://translate.google.com/#{0}/{1}/{2}", _fromLang.Code, _toLang.Code, _textToTranslate);
                Process.Start(translateUrl);
                return true;
            }
        }

        public string GetIcon()
        {
            return "translate.png";
        }
    }
}
