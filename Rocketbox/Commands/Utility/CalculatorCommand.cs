using System;
using System.Collections.Generic;
using NCalc;

namespace Rocketbox
{
    /// <summary>
    /// Command for in-line equations
    /// </summary>
    internal class CalculatorCommand : IRbCommand
    {
        private string _result;

        public List<string> Keywords { get; } = new List<string> { "=" };

        internal CalculatorCommand()
        {
            _result = string.Empty;
        }

        public string GetResponse(string arguments)
        {
            try
            {
                // Math is processed by ncalc
                Expression expr = new Expression(arguments);
                _result = Convert.ToDouble(expr.Evaluate()).ToString();
            }
            catch (Exception)
            {
                _result = "Error";
            }

            return string.Format("=   {0}", _result);
        }

        // copy the calculated value to the clipboard
        public bool Execute(string arguments)
        {
            System.Windows.Clipboard.SetText(_result);
            return false;
        }

        public string GetIcon()
        {
            return "calculator.png";
        }
    }
}
