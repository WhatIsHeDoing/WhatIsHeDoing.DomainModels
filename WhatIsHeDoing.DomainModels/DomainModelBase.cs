namespace WhatIsHeDoing.DomainModels
{
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Domain model base contract.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public abstract class DomainModelBase<T> : IDomainModel<T>
    {
        public T Value { get; protected set; }

        public abstract IDomainModel<T> AssignFrom(object value);

        // <summary>
        /// Determines whether this domain model is identical to another.
        /// </summary>
        /// <param name="obj">To compare</param>
        /// <returns><c>true</c> if both values are identical</returns>
        public override bool Equals(object obj)
        {
            var other = obj as DomainModelBase<T>;
            return other == null ? base.Equals(obj) : GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Gets the hash code of this domain model value.
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode() => BitConverter.ToInt32
            (MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Value.ToString())), 0);

        /// <summary>
        /// Gets the string representation of this domain model.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString() => Value.ToString();

        public abstract bool TryValidate(T value);
        public abstract IDomainModel<T> Validate(T value);

        /// <summary>
        /// Operator that converts a domain model to a string.
        /// </summary>
        /// <param name="source">To convert</param>
        /// <returns>String</returns>
        public static implicit operator string(DomainModelBase<T> source) =>
            source != null
            ? source.ToString()
            : throw new ArgumentNullException(nameof(source));
    }
}
