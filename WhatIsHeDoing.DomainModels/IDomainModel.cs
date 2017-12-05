namespace WhatIsHeDoing.DomainModels
{
    using System.Xml.Serialization;

    /// <summary>
    /// Domain model contract, used to enforce useful
    /// common features and de/serialisation from JSON and XML.
    /// </summary>
    public interface IDomainModel<T> : IXmlSerializable
    {
        /// <summary>
        /// Raw value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Constructs this model from a value.
        /// </summary>
        /// <param name="value">From which to validate and construct</param>
        /// <returns>This</returns>
        IDomainModel<T> Construct(object value);

        /// <summary>
        /// Constructs this model from a value.
        /// </summary>
        /// <param name="value">From which to validate and construct</param>
        /// <returns>This</returns>
        IDomainModel<T> Construct(T value);

        /// <summary>
        /// Converts the model to string.
        /// </summary>
        /// <returns>String respresentation</returns>
        string ToString();
    }
}
