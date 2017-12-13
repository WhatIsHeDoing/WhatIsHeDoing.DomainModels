namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// International Article Number.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/International_Article_Number"/>
    /// <example>73513537</example>
    /// <remarks>Originally European Article Number</remarks>
    [JsonConverter(typeof(DomainModelJSONConverter<EAN, ulong>))]
    [TypeConverter(typeof(DomainModelTypeConverter<EAN, ulong>))]
    public class EAN : DomainModelBase<ulong>, IBarcode
    {
        private static readonly ulong[] ValidLengths =
            new[] { 8UL, 12UL, 13UL, 14UL, 18UL };

        // Parameterless constructor required for serialisation.
        public EAN()
        {
        }

        public EAN(ulong value)
            : base(value)
        {
        }

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
            ValidLengths.Contains(barcode.Length()) &&
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

        public override IDomainModel<ulong> Construct(object source)
        {
            if (!ulong.TryParse(source as string, out ulong value))
            {
                throw new DomainValueException();
            }

            return Construct(value);
        }

        public override IDomainModel<ulong> Construct(ulong source)
        {
            if (!IsValid(source))
            {
                throw new DomainValueException(nameof(source));
            }

            Value = source;
            return this;
        }

        public override void ReadXml(XmlReader reader)
        {
            if (!ulong.TryParse(reader.ReadElementContentAsString(), out ulong value))
            {
                throw new DomainValueException();
            }

            Construct(value);
        }
    }
}
