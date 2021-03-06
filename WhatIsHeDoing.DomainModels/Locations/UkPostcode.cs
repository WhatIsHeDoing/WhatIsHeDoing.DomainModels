namespace WhatIsHeDoing.DomainModels.Locations
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// Models a UK postcode and its constituent parts.
    /// <seealso cref="http://en.wikipedia.org/wiki/Postcodes_in_the_United_Kingdom"/>
    /// <example>W1A ONY</example>
    /// <remarks>Will only validate, not verify!</remarks>
    /// </summary>
    [JsonConverter(typeof(DomainModelJSONConverter<UKPostcode, string>))]
    [TypeConverter(typeof(DomainModelTypeConverter<UKPostcode, string>))]
    public class UKPostcode : DomainModelBase<string>
    {
        private const int InwardCodeLength = 3;
        private const string OutwardInwardCodesSeparator = " ";

        private static readonly Regex NumberRegex = new Regex(@"\d");

        private static readonly Regex ValidationRegex = new Regex(
            "(GIR\\s?0AA)|" +
            "((([A-Z-[QVX]][0-9][0-9]?)|" +
            "(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|" +
            "(([A-Z-[QVX‌​]][0-9][A-HJKSTUW])|" +
            "([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY]))))" +
            "\\s?[0-9][A-Z-[C‌​IKMOV]]{2})");

        // Parameterless constructor required for serialisation.
        public UKPostcode()
        {
        }

        public UKPostcode(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Part of the postcode before the separator in the middle.
        /// </summary>
        /// <example>W1A</example>
        public string OutwardCode { get; private set; }

        /// <summary>
        /// Part of the postcode after the separator in the middle.
        /// </summary>
        /// <example>0NY</example>
        public string InwardCode { get; private set; }

        /// <summary>
        /// Part of the outward code. The postcode area is between
        /// one and two characters long and is all letters.
        /// </summary>
        /// <example>W</example>
        public string PostcodeArea { get; private set; }

        /// <summary>
        /// Part of the outward code. It is one or two digits
        /// (and sometimes a final letter) that are added
        /// to the end of the postcode area.
        /// </summary>
        /// <example>1A</example>
        public string PostcodeDistrict { get; private set; }

        /// <summary>
        /// Made up of the postcode district, the separator,
        /// and the first character of the inward code.
        /// </summary>
        /// <example>W1A 0</example>
        public string PostcodeSector { get; private set; }

        /// <summary>
        /// Two characters added to the end of the postcode sector.
        /// </summary>
        /// <example>NY</example>
        public string PostcodeUnit { get; private set; }

        public static bool IsValid(string value) =>
            ValidationRegex.IsMatch(Clean(value));

        public static bool TryParse(string source, out UKPostcode model)
        {
            if (!IsValid(source))
            {
                model = null;
                return false;
            }

            model = new UKPostcode(source);
            return true;
        }

        public override IDomainModel<string> Construct(object value) =>
            Construct(Convert.ToString(value));

        public override void ReadXml(XmlReader reader) =>
            Construct(reader.ReadElementContentAsString());

        public override IDomainModel<string> Construct(string ukPostcode)
        {
            // Bomb out if this is not valid.
            ukPostcode = Clean(ukPostcode);

            if (!IsValid(ukPostcode))
            {
                throw new DomainValueException(nameof(ukPostcode));
            }

            // Calculate and cache the outward code length.
            var outwardCodeLength = ukPostcode.Length - InwardCodeLength;

            // Set the individual elements of the postcode.
            OutwardCode = ukPostcode.Substring(0, outwardCodeLength);
            PostcodeArea = NumberRegex.Split(OutwardCode).First();
            PostcodeDistrict = OutwardCode.Substring(PostcodeArea.Length);
            InwardCode = ukPostcode.Substring(outwardCodeLength);

            PostcodeSector = OutwardCode +
                    OutwardInwardCodesSeparator + InwardCode.Substring(0, 1);

            PostcodeUnit = InwardCode.Substring(1, 2);

            // Store the entire postcode too.
            Value = OutwardCode + OutwardInwardCodesSeparator + InwardCode;

            return this;
        }

        private static string Clean(string value) => value
            ?.Trim()
            ?.Replace(OutwardInwardCodesSeparator, string.Empty)
            ?.ToUpper();
    }
}
