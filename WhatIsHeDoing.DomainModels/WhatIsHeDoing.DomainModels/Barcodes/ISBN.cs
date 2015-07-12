using System;
using System.Diagnostics;
using System.Globalization;
using WhatIsHeDoing.Core.Extensions;
    
namespace WhatIsHeDoing.DomainModels.Barcodes
{
    /// <summary>
    /// International Standard Book Number.
    /// </summary>
    [DebuggerDisplay("{_value}")]
    public class ISBN : IBarcode
    {
        private static readonly CultureInfo _culture
            = CultureInfo.CurrentCulture;

        private readonly long _value;

        /// <summary>
        /// Creates an ISBN from a barcode.
        /// </summary>
        /// <param name="barcode">To use</param>
        public ISBN(string barcode)
        {
            if (!IsValid(barcode))
            {
                throw new ArgumentException("Invalid barcode", "barcode");
            }

            _value = long.Parse(barcode, _culture);
        }

        /// <summary>
        /// Determines whether a barcode is a valid ISBN.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Success</returns>
        public static bool IsValid(string barcode)
        {
            // As an ISBN is a superset of EAN, check that is still valid.
            if (!EAN.IsValid(barcode))
            {
                return false;
            }

            long value;

            if (!long.TryParse(barcode, out value))
            {
                return false;
            }

            // Ensure the first three digits are recognised values.
            var firstThreeDigits = value.StripDigits(10);
            return firstThreeDigits == 978 || firstThreeDigits == 979;
        }

        /// <summary>
        /// Gets the string representation of this ISBN.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return _value.ToString(_culture);
        }
    }
}
