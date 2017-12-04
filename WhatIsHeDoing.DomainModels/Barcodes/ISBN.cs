namespace WhatIsHeDoing.DomainModels.Barcodes
{
    using Core.Extensions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// International Standard Book Number.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/International_Standard_Book_Number"/>
    /// <example>978-3-16-148410-0</example>
     public class ISBN : DomainModelBase<ulong>, IBarcode
    {
        /// <summary>
        /// Parameterless constructor required for serialisation.
        /// </summary>
        public ISBN() { }

        /// <summary>
        /// Creates an ISBN from a barcode.
        /// </summary>
        /// <param name="barcode">To use</param>
        public ISBN(ulong barcode) => Value = IsValid(barcode)
            ? barcode
            : throw new ArgumentException(nameof(barcode));

        /// <summary>
        /// Assigns the value from another ISBN. Used in deserialisation.
        /// </summary>
        /// <param name="value">From which to assign</param>
        /// <returns>This model</returns>
        public override IDomainModel<ulong> AssignFrom(object value)
        {
            var assigner = new ISBN(Convert.ToUInt64(value));
            Value = assigner.Value;
            return this;
        }

        /// <summary>
        /// Non-throwing validation of an ISBN.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Validity</returns>
        public override bool TryValidate(ulong value) => IsValid(value);

        /// <summary>
        /// Validates and returns an ISBN model or throws an error if it is not valid.
        /// </summary>
        /// <param name="value">To test</param>
        /// <returns>Model</returns>
        /// <exception cref="ArgumentException">If not valid</exception>
        public override IDomainModel<ulong> Validate(ulong value) => IsValid(value)
            ? new ISBN(value)
            : throw new ArgumentException(nameof(value));

        /// <summary>
        /// Determines whether a barcode is a valid ISBN.
        /// </summary>
        /// <param name="barcode">To test</param>
        /// <returns>Validity</returns>
        public static bool IsValid(ulong barcode) =>
            barcode.Length() == VALID_LENGTH &&
            _validStartSequences.Contains(barcode.StripDigits(10));

        /// <summary>
        /// Attempts to parse an ISBN
        /// and sets it as the out parameter on success.
        /// </summary>
        /// <param name="data">To parse</param>
        /// <param name="source">To set; will be null on failure</param>
        /// <returns>Success</returns>
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
