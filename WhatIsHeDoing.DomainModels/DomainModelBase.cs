namespace WhatIsHeDoing.DomainModels
{
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// Domain model base contract.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public abstract class DomainModelBase<T> : IDomainModel<T>
    {
        /// <summary>
        /// Underlying value of the model.
        /// </summary>
        public T Value { get; protected set; }

        /// <remarks>
        /// Parameterless constructor required for serialisation.
        /// </remarks>
        public DomainModelBase() { }

        /// <summary>
        /// Constructor that creates a domain model from data.
        /// </summary>
        /// <param name="value">From which to validate and construct</param>
        public DomainModelBase(T value) => Construct(value);

        public abstract IDomainModel<T> Construct(object value);
        public abstract IDomainModel<T> Construct(T value);

        /// <summary>
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
        /// Warning: not used!
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() => null;

        public abstract void ReadXml(XmlReader reader);

        /// <summary>
        /// Gets the string representation of this domain model.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// Serialises the domain model value to XML.
        /// </summary>
        /// <param name="writer">XML writer</param>
        public void WriteXml(XmlWriter writer) => writer.WriteValue(Value);

        /// <summary>
        /// Operator that converts a domain model to a string.
        /// </summary>
        /// <param name="source">To convert</param>
        /// <returns>String</returns>
        public static implicit operator string(DomainModelBase<T> source)
            => source.ToString();
    }
}
