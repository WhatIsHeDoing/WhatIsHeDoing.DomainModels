using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace WhatIsHeDoing.DomainModels.Barcodes
{
    /// <summary>
    /// International Article Number.
    /// </summary>
    /// <remarks>Originally European Article Number</remarks>
    [DebuggerDisplay("{_value}")]
    public class EAN : IBarcode
    {
        private readonly long _value;

        private static readonly CultureInfo _culture
            = CultureInfo.CurrentCulture;

        private static readonly int[] _validLengths =
            new[] { 8, 12, 13, 14, 18 };


        /// <summary>
        /// Creates an ISBN from a barcode.
        /// </summary>
        /// <param name="barcode">To use</param>
        public EAN(string barcode) => _value = IsValid(barcode)
            ? long.Parse(barcode, _culture)
            : throw new ArgumentException(nameof(barcode));

        /// <summary>
        /// Gets the string representation of this EAN.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString() => _value.ToString(_culture);

        /// <summary>
        /// Implicit string operator.
        /// </summary>
        /// <param name="ean">To coerce</param>
        /// <returns>String or null if the EAN is null</returns>
        public static implicit operator string(EAN ean) => ean?.ToString();

        /// <summary>
        /// Determines whether a barcode is a valid EAN.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Success</returns>
        public static bool IsValid(string barcode) =>
            !String.IsNullOrWhiteSpace(barcode) &&
            long.TryParse(barcode, out long value) &&
            _validLengths.Contains(barcode.Length) &&
            HasValidChecksum(barcode);

        /// <summary>
        /// Converts a barcode to an EAN.
        /// </summary>
        /// <param name="barcode">To convert</param>
        /// <param name="ean">EAN; null on failure</param>
        /// <returns>Success</returns>
        public static bool TryParse(string barcode, out EAN ean)
        {
            ean = null;

            if (!IsValid(barcode))
            {
                return false;
            }

            ean = new EAN(barcode);
            return true;
        }

        /// <summary>
        /// Determines whether an EAN has a valid checksum.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Validity</returns>
        public static bool HasValidChecksum(string barcode)
        {
            // Ignore an empty barcode.
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return false;
            }

            // Create an array of all digits.
            var barcodeInts = barcode
                .ToCharArray()
                .Select(c => Convert.ToInt32
                    (Convert.ToString(c, _culture), _culture));

            // Take all but the checksum.
            var barcodeIntsNoChecksum = barcodeInts
                .Take(barcode.Length - 1)
                .ToList();

            // Insert a leading zero if the length of the barcode is odd.
            // This is to ensure the multiplication factor is consistent
            // across all barcode lengths.
            if (barcodeIntsNoChecksum.Count % 2 == 1)
            {
                barcodeIntsNoChecksum.Insert(0, 0);
            }

            // Sum the even indexes.
            var evens = barcodeIntsNoChecksum
                .Where((i, n) => n % 2 == 0)
                .Sum();

            // Sum the even index values multiplied by three.
            var odds = barcodeIntsNoChecksum
                .Where((i, n) => n % 2 == 1)
                .Select(i => i * 3)
                .Sum();

            // Sum the evens and odds and calculate the difference
            // with that value rounded to the nearest ten.
            var total = odds + evens;
            var totalToNearestTen = ((int)Math.Ceiling(total / 10.0)) * 10;
            var totalDifference = totalToNearestTen - total;

            // Finally, compare this to the actual checksum!
            return totalDifference == barcodeInts.Last();
        }
    }
}
