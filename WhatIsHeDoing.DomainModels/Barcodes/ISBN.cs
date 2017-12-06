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
        /// <remarks>
        /// Parameterless constructor required for serialisation.
        /// </remarks>
        public ISBN() { }

        public ISBN(ulong value) : base(value) { }

        public override IDomainModel<ulong> Construct(object value) =>
            Construct(Convert.ToUInt64(value));

        public override void ReadXml(XmlReader reader) =>
            Construct(Convert.ToUInt64(reader.ReadElementContentAsString()));

        public override IDomainModel<ulong> Construct(ulong source)
        {
            if (!IsValid(source))
            {
                throw new ArgumentException(nameof(source));
            }

            Value = source;
            return this;
        }

        public static bool IsValid(ulong barcode) =>
            barcode.Length() == VALID_LENGTH &&
            _validStartSequences.Contains(barcode.StripDigits(10));

        public static bool TryParse(ulong data, out ISBN source)
        {
            source = null;

            if (!IsValid(data))
            {
                return false;
            }

            source = new ISBN(data);
            return true;
        }

        private const ulong VALID_LENGTH = 13;

        private static readonly IList<ulong> _validStartSequences = new[]
        {
            978UL,
            979UL
        };
    }
}
