namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml;

    /// <summary>
    /// International Standard Book Number.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/International_Standard_Book_Number"/>
    /// <example>978-3-16-148410-0</example>
    [JsonConverter(typeof(DomainModelJSONConverter<ISBN, ulong>))]
    [TypeConverter(typeof(DomainModelTypeConverter<ISBN, ulong>))]
    public class ISBN : DomainModelBase<ulong>, IBarcode
    {
        private const ulong ValidLength = 13;

        private static readonly IList<ulong> ValidStartSequences = new[]
        {
            978UL,
            979UL
        };

        // Parameterless constructor required for serialisation.
        public ISBN()
        {
        }

        public ISBN(ulong value)
            : base(value)
        {
        }

        public static bool IsValid(ulong barcode) =>
            barcode.Length() == ValidLength &&
            ValidStartSequences.Contains(barcode.StripDigits(10));

        public static bool TryParse(ulong data, out ISBN source)
        {
            if (!IsValid(data))
            {
                source = null;
                return false;
            }

            source = new ISBN(data);
            return true;
        }

        public override IDomainModel<ulong> Construct(object value)
        {
            if (!ulong.TryParse(value as string, out ulong parsed))
            {
                throw new DomainValueException();
            }

            return Construct(parsed);
        }

        public override IDomainModel<ulong> Construct(ulong value)
        {
            if (!IsValid(value))
            {
                throw new DomainValueException(nameof(value));
            }

            Value = value;
            return this;
        }

        public override void ReadXml(XmlReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            if (!ulong.TryParse(reader.ReadElementContentAsString(), out ulong value))
            {
                throw new DomainValueException();
            }

            Construct(value);
        }
    }
}
