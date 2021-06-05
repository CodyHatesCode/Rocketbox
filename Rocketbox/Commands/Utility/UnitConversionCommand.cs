using System;
using System.Linq;

namespace Rocketbox
{
    /// <summary>
    /// Command for converting between units of measurement.
    /// </summary>
    internal class UnitConversionCommand : IRbCommand
    {
        private RbConversionUnit _convertFrom;
        private RbConversionUnit _convertTo;

        private decimal _valueFrom;

        private decimal _result;

        internal UnitConversionCommand() { }

        public string GetResponse(string arguments)
        {
            bool isError = false;

            string[] parts = arguments.Split(' ');

            if (parts.Length < 3)
            {
                return "Unit conversion:   Not enough parameters.";
            }

            // first and second part = number/unit
            if (!decimal.TryParse(parts[0], out _valueFrom))
            {
                isError = true;
            }

            _convertFrom = RbData.GetConversionUnit(parts[1]);

            // last part = output unit
            // (can use "to" or other combos in the middle)
            _convertTo = RbData.GetConversionUnit(parts.Last());


            if (_convertFrom.GetUnitType() == RbUnitType.Null || _convertTo.GetUnitType() == RbUnitType.Null)
            {
                isError = true;
            }
            else if (_convertFrom.Type != _convertTo.Type)
            {
                return "Unit conversion:   Unit type mismatch.";
            }


            if (isError)
            {
                return "Unit conversion:   Cannot convert.";
            }
            else
            {
                _result = (_valueFrom * _convertFrom.Multiplier) / _convertTo.Multiplier;
                return string.Format("Unit conversion:   {0} {1} = {2} {3}", _valueFrom, _convertFrom.Name, _result.ToString("0.#####"), _convertTo.Name);
            }
        }

        public bool Execute(string arguments)
        {
            System.Windows.Clipboard.SetText(_result.ToString("0.#####"));
            return false;
        }

        public string GetIcon()
        {
            return string.Empty;
        }
    }
}
