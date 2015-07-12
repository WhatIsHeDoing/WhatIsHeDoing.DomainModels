using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace WhatIsHeDoing.DomainModels.Locations
{
    /// <summary>
    /// Models a UK postcode and its constituent parts.
    /// <seealso cref="http://en.wikipedia.org/wiki/Postcodes_in_the_United_Kingdom"/>
    /// <example>W1A 0NY</example>
    /// <remarks>Will only validate, not verify!</remarks>
    /// </summary>
    public class UKPostcode
    {
        /// <summary>
        /// Regex used to validate a postcode.
        /// </summary>
        public const string ValidationRegexPattern =
            "(GIR\\s?0AA)|" +
            "((([A-Z-[QVX]][0-9][0-9]?)|" +
            "(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|" +
            "(([A-Z-[QVX‌​]][0-9][A-HJKSTUW])|" +
            "([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY]))))" +
            "\\s?[0-9][A-Z-[C‌​IKMOV]]{2})";

        /// <summary>
        /// Regex used to validate a postcode.
        /// </summary>
        private static Regex ValidationRegex = new Regex(ValidationRegexPattern);

        /// <summary>
        /// Length of the inward code.
        /// </summary>
        public const int InwardCodeLength = 3;

        /// <summary>
        /// Separator of the inward and outward codes.
        /// </summary>
        public const string OutwardInwardCodesSeparator = " ";

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

        private readonly string _value;

        /// <summary>
        /// Constructor that creates a UK postcode object from data.
        /// </summary>
        /// <param name="ukPostCode">To inspect</param>
        /// <exception cref="ArgumentException">
        /// Thrown when a postcode is invalid.
        /// </exception>
        public UKPostcode(string ukPostcode)
        {
            if (String.IsNullOrWhiteSpace(ukPostcode))
            {
                throw new ArgumentNullException("ukPostcode");
            }

            // Trim whitespace, remove the separator and convert to uppercase.
            ukPostcode = ukPostcode.Trim().Replace
                (OutwardInwardCodesSeparator, String.Empty).ToUpper();

            // Bomb out if this is not valid.
            if (!IsValid(ukPostcode))
            {
                throw new ArgumentException("postcode");
            }

            // Calculate and cache the outward code length.
            var outwardCodeLength = ukPostcode.Length - InwardCodeLength;

            // Set the individual elements of the postcode.
            OutwardCode = ukPostcode.Substring(0, outwardCodeLength);

            PostcodeArea = String.Concat
                (OutwardCode.ToCharArray().Where(char.IsLetter));

            PostcodeDistrict = OutwardCode.Substring(PostcodeArea.Length);
            InwardCode = ukPostcode.Substring(outwardCodeLength);

            PostcodeSector = OutwardCode +
                    OutwardInwardCodesSeparator + InwardCode.Substring(0, 1);

            PostcodeUnit = InwardCode.Substring(1, 2);

            // Store the entire postcode too.
            _value = OutwardCode + OutwardInwardCodesSeparator + InwardCode;
        }

        /// <summary>
        /// Determines whether a postcode is valid.
        /// </summary>
        /// <param name="postcode">To validate</param>
        /// <returns>Success</returns>
        public static bool IsValid(string postcode)
        {
            return ValidationRegex.IsMatch(postcode);
        }

        /// <summary>
        /// Attempts to parse a postcode
        /// and sets it as the out parameter on success.
        /// </summary>
        /// <param name="data">To parse</param>
        /// <param name="ukPostCode">To set; will be null on failure</param>
        /// <returns>Success</returns>
        public static bool TryParse
            (string data, out UKPostcode ukPostcode)
        {
            ukPostcode = null;

            if (!IsValid(data))
            {
                return false;
            }

            ukPostcode = new UKPostcode(data);
            return true;
        }

        /// <summary>
        /// Operator that converts a postcode to a string.
        /// </summary>
        /// <param name="ukPostcode">To convert</param>
        /// <returns>String</returns>
        [SuppressMessage("Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "It is validated!")]
        public static implicit operator string(UKPostcode ukPostcode)
        {
            return String.IsNullOrWhiteSpace(ukPostcode)
                ? ukPostcode
                : ukPostcode.ToString();
        }

        /// <summary>
        /// Determines whether this postcode is identical to another.
        /// </summary>
        /// <param name="obj">To compare</param>
        /// <returns><c>true</c> if both values are identical</returns>
        public override bool Equals(object obj)
        {
            var other = obj as UKPostcode;
            return (other == null) ? base.Equals(obj) : _value == other._value;
        }

        /// <summary>
        /// Generates the string representation of this postcode.
        /// </summary>
        /// <returns>Postcode</returns>
        /// <example>W1A 0NY</example>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>
        /// Gets the hash code of this postcode value.
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
