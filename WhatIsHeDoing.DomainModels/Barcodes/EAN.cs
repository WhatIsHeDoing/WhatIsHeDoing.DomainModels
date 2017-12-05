namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using System;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// International Article Number.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/International_Article_Number"/>
    /// <example>4006381333937</example>
    /// <remarks>Originally European Article Number</remarks>
    public class EAN : DomainModelBase<ulong>, IBarcode
    {
        /// <remarks>
        /// Parameterless constructor required for serialisation.
        /// </remarks>
        public EAN() { }

        public EAN(ulong value) : base(value) { }
        
        public override IDomainModel<ulong> Construct(object value)
            => Construct(Convert.ToUInt64(value));

        public override IDomainModel<ulong> Construct(ulong source)
        {
            if (!IsValid(source))
            {
                throw new InvalidOperationException();
            }

            Value = source;
            return this;
        }

        public override void ReadXml(XmlReader reader) =>
            Construct(Convert.ToUInt64(reader.ReadElementContentAsString()));

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

        public static bool IsValid(ulong barcode) =>
            _validLengths.Contains(barcode.Length()) &&
            HasValidChecksum(barcode);

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
