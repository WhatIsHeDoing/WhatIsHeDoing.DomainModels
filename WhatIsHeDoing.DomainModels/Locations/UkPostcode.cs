using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WhatIsHeDoing.DomainModels.Locations
{
    /// <summary>
    /// Models a UK postcode and its constituent parts.
    /// <seealso cref="http://en.wikipedia.org/wiki/Postcodes_in_the_United_Kingdom"/>
    /// <example>W1A ONY</example>
    /// <remarks>Will only validate, not verify!</remarks>
    /// </summary>
    public class UKPostcode : DomainModelBase<string>
    {
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

        /// <summary>
        /// Parameterless constructor required for serialisation.
        /// </summary>
        public UKPostcode() { }

        /// <summary>
        /// Constructor that creates a UK postcode object from data.
        /// </summary>
        /// <param name="ukPostCode">To inspect</param>
        /// <exception cref="ArgumentException">
        /// Thrown when a postcode is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when an empty value is supplied.
        /// </exception>
        public UKPostcode(string ukPostcode)
        {
            // Bomb out if this is not valid.
            if (string.IsNullOrWhiteSpace(ukPostcode))
            {
                throw new ArgumentNullException(nameof(ukPostcode));
            }

            ukPostcode = _clean(ukPostcode);

            if (!IsValid(ukPostcode))
            {
                throw new ArgumentException(nameof(ukPostcode));
            }

            // Calculate and cache the outward code length.
            var outwardCodeLength = ukPostcode.Length - INWARD_CODE_LENGTH;

            // Set the individual elements of the postcode.
            OutwardCode = ukPostcode.Substring(0, outwardCodeLength);
            PostcodeArea = _numberRegex.Split(OutwardCode).First();
            PostcodeDistrict = OutwardCode.Substring(PostcodeArea.Length);
            InwardCode = ukPostcode.Substring(outwardCodeLength);

            PostcodeSector = OutwardCode +
                    OUTWARD_INWARD_CODES_SEPARATOR + InwardCode.Substring(0, 1);

            PostcodeUnit = InwardCode.Substring(1, 2);

            // Store the entire postcode too.
            Value = OutwardCode + OUTWARD_INWARD_CODES_SEPARATOR + InwardCode;
        }

        /// <summary>
        /// Assigns the value from another postcode. Used in deserialisation.
        /// </summary>
        /// <param name="value">From which to assign</param>
        /// <returns>This model</returns>
        public override IDomainModel<string> AssignFrom(object value)
        {
            var assigner = new UKPostcode(Convert.ToString(value));
            Value = assigner.Value;
            return this;
        }

        /// <summary>
        /// Generates the string representation of this postcode.
        /// </summary>
        /// <returns>Postcode</returns>
        public override string ToString() => Value;

        /// <summary>
        /// Non-throwing validation of a postcode.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Validity</returns>
        public override bool TryValidate(string value) => IsValid(value);

        /// <summary>
        /// Validates and returns a postcode model or throws an error if it is not valid.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Model</returns>
        /// <exception cref="ArgumentException">If not valid</exception>
        public override IDomainModel<string> Validate(string value) => IsValid(value)
            ? new UKPostcode(value)
            : throw new ArgumentException(nameof(value));

        /// <summary>
        /// Determines whether a string is a valid postcode.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Validity</returns>
        public static bool IsValid(string value) =>
            _validationRegex.IsMatch(_clean(value));

        /// <summary>
        /// Attempts to parse a postcode
        /// and sets it as the out parameter on success.
        /// </summary>
        /// <param name="source">To parse</param>
        /// <param name="model">To set; will be null on failure</param>
        /// <returns>Success</returns>
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

        private const int INWARD_CODE_LENGTH = 3;
        private const string OUTWARD_INWARD_CODES_SEPARATOR = " ";

        private static readonly Regex _numberRegex = new Regex(@"\d");

        private static readonly Regex _validationRegex = new Regex(
            "(GIR\\s?0AA)|" +
            "((([A-Z-[QVX]][0-9][0-9]?)|" +
            "(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|" +
            "(([A-Z-[QVX‌​]][0-9][A-HJKSTUW])|" +
            "([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY]))))" +
            "\\s?[0-9][A-Z-[C‌​IKMOV]]{2})");

        private static string _clean(string value) => value
            .Trim()
            .Replace(OUTWARD_INWARD_CODES_SEPARATOR, string.Empty)
            .ToUpper();
    }
}
