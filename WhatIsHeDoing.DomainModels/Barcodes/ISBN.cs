namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using System;
    using System.Diagnostics;
    using System.Globalization;

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
        public ISBN(string barcode) => _value = IsValid(barcode)
            ? long.Parse(barcode, _culture)
            : throw new ArgumentException(nameof(barcode));

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

            if (!long.TryParse(barcode, out long value))
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
        public override string ToString() => _value.ToString(_culture);
    }
}
