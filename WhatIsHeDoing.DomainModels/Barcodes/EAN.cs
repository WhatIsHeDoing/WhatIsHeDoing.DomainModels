namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using System;
    using System.Linq;

    /// <summary>
    /// International Article Number.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/International_Article_Number"/>
    /// <example>4006381333937</example>
    /// <remarks>Originally European Article Number</remarks>
    public class EAN : DomainModelBase<ulong>, IBarcode
    {
        /// <summary>
        /// Parameterless constructor required for serialisation.
        /// </summary>
        public EAN() { }

        /// <summary>
        /// Creates an ISBN from a barcode.
        /// </summary>
        /// <param name="barcode">To use</param>
        public EAN(ulong barcode) => Value = IsValid(barcode)
            ? barcode
            : throw new ArgumentException(nameof(barcode));

        /// <summary>
        /// Assigns the value from another ISBN. Used in deserialisation.
        /// </summary>
        /// <param name="value">From which to assign</param>
        /// <returns>This model</returns>
        public override IDomainModel<ulong> AssignFrom(object value)
        {
            var assigner = new EAN(Convert.ToUInt64(value));
            Value = assigner.Value;
            return this;
        }
        
        /// <summary>
        /// Non-throwing validation of an EAN.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Validity</returns>
        public override bool TryValidate(ulong value) => IsValid(value);

        /// <summary>
        /// Validates and returns an EAN model or throws an error if it is not valid.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Model</returns>
        /// <exception cref="ArgumentException">If not valid</exception>
        public override IDomainModel<ulong> Validate(ulong value) => IsValid(value)
            ? new EAN(value)
            : throw new ArgumentException(nameof(value));

        /// <summary>
        /// Determines whether an EAN has a valid checksum.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Validity</returns>
        public static bool HasValidChecksum(ulong barcode)
        {
            // Create an array of all digits.
            var barcodeInts = barcode
                .ToString()
                .Select(c => Convert.ToInt32(Convert.ToString(c)));

            // Take all but the checksum.
            var barcodeIntsNoChecksum = barcodeInts
                .Take(Convert.ToInt32(barcode.Length()) - 1)
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

            // Sum the evens and odds, calculate the difference
            // with that value rounded to the nearest ten,
            // and compare this to the actual checksum.
            var total = odds + evens;
            return (total.ToNearestCeiling(10) - total) == barcodeInts.Last();
        }

        /// <summary>
        /// Determines whether a barcode is a valid EAN.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Success</returns>
        public static bool IsValid(ulong barcode) =>
            _validLengths.Contains(barcode.Length()) &&
            HasValidChecksum(barcode);

        /// <summary>
        /// Attempts to parse an EAN
        /// and sets it as the out parameter on success.
        /// </summary>
        /// <param name="source">To parse</param>
        /// <param name="model">To set; will be null on failure</param>
        /// <returns>Success</returns>
        public static bool TryParse(ulong source, out EAN model)
        {
            if (!IsValid(source))
            {
                model = null;
                return false;
            }

            model = new EAN(source);
            return true;
        }

        private static readonly ulong[] _validLengths =
            new[] { 8UL, 12UL, 13UL, 14UL, 18UL };
    }
}
