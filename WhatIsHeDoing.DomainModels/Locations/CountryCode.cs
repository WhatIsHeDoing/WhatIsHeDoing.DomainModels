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
        private static readonly Regex ISOValidatationRegex =
            new Regex(@"^[a-zA-Z]{2,3}$");

        // Parameterless constructor required for serialisation.
        public CountryCode()
        {
        }

        public CountryCode(string value)
            : base(value)
        {
        }

        public static bool IsValid(string source) =>
            !string.IsNullOrWhiteSpace(source) && ISOValidatationRegex.IsMatch(source);

        public static bool TryParse(string source, out CountryCode model)
        {
            if (!IsValid(source))
            {
                model = null;
                return false;
            }

            model = new CountryCode(source);
            return true;
        }

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
    }
}
