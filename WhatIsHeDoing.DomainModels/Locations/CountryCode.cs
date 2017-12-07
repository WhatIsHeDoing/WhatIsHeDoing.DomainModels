namespace WhatIsHeDoing.DomainModels.Locations
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// Two and three letter country codes.
    /// </summary>
    /// <example>GB</example>
    /// <seealso>
    /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
    /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
    /// </seealso>
    [JsonConverter(typeof(DomainModelJSONConverter<CountryCode, string>))]
    [TypeConverter(typeof(DomainModelTypeConverter<CountryCode, string>))]
    public class CountryCode : DomainModelBase<string>
    {
        /// <remarks>
        /// Parameterless constructor required for serialisation.
        /// </remarks>
        public CountryCode() { }

        public CountryCode(string value) : base(value) { }

        public override IDomainModel<string> Construct(object value) =>
            Construct(Convert.ToString(value));
        
        public override IDomainModel<string> Construct(string source)
        {
            if (!IsValid(source))
            {
                throw new DomainValueException(nameof(source));
            }

            Value = source.ToUpper();
            return this;
        }

        public override void ReadXml(XmlReader reader) =>
            Construct(reader.ReadElementContentAsString());

        public override string ToString() => Value;

        private static readonly Regex _isoValidatationRegex =
            new Regex(@"^[a-zA-Z]{2,3}$");

        public static bool IsValid(string source) =>
            !string.IsNullOrWhiteSpace(source) && _isoValidatationRegex.IsMatch(source);

    }
}
