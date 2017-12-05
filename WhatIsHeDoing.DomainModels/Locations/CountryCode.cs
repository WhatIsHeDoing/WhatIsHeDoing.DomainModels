namespace WhatIsHeDoing.DomainModels.Locations
{
    using System;
    using System.Text.RegularExpressions;
    using System.Xml;

    public class CountryCode : DomainModelBase<string>
    {
        /// <summary>
        /// Parameterless constructor required for serialisation.
        /// </summary>
        public CountryCode() { }

        /// <summary>
        /// Two and three letter country codes.
        /// </summary>
        /// <example>GB</example>
        /// <seealso>
        /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
        /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
        /// </seealso>
        public CountryCode(string source) => _construct(source);

        /// <summary>
        /// Assigns the value from another country code. Used in deserialisation.
        /// </summary>
        /// <param name="value">From which to assign</param>
        /// <returns>This model</returns>
        public override IDomainModel<string> AssignFrom(object value) =>
            _construct(Convert.ToString(value));

        /// <summary>
        /// Read and assign a value from XML.
        /// </summary>
        /// <param name="reader">XML reader</param>
        public override void ReadXml(XmlReader reader) =>
            _construct(reader.ReadElementContentAsString());

        /// <summary>
        /// Non-throwing validation of a country code.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Validity</returns>
        public override bool TryValidate(string value) => IsValid(value);

        /// <summary>
        /// Generates the string representation of this postcode.
        /// </summary>
        /// <returns>Postcode</returns>
        public override string ToString() => Value;

        /// <summary>
        /// Validates and returns a country code or throws an error if it is not valid.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Model</returns>
        /// <exception cref="ArgumentException">If not valid</exception>
        public override IDomainModel<string> Validate(string value) => IsValid(value)
            ? new CountryCode(value)
            : throw new ArgumentException(nameof(value));

        private IDomainModel<string> _construct(string source)
        {
            if (!IsValid(source))
            {
                throw new ArgumentException(nameof(source));
            }

            Value = source.ToUpper();
            return this;
        }

        private static readonly Regex _isoValidatationRegex =
            new Regex(@"^[a-zA-Z]{2,3}$");

        public static bool IsValid(string source) =>
            !string.IsNullOrWhiteSpace(source) && _isoValidatationRegex.IsMatch(source);

    }
}
