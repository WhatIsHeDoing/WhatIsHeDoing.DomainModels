using Newtonsoft.Json;
using System;

namespace WhatIsHeDoing.DomainModels
{
    /// <summary>
    /// Converts any object to a domain model.
    /// </summary>
    /// <typeparam name="TDomainModel">Domain model type</typeparam>
    /// <typeparam name="TValue">Domain model value</typeparam>
    public class DomainModelConverter<TDomainModel, TValue> :
        JsonConverter where TDomainModel : IDomainModel<TValue>, new()
    {
        /// <summary>
        /// Always assumes it can convert any object.
        /// </summary>
        /// <param name="objectType">Not used</param>
        /// <returns>True</returns>
        public override bool CanConvert(Type objectType) => true;

        /// <summary>
        /// Creates a domain model from an object.
        /// </summary>
        /// <param name="reader">From which the value is extracted</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="serializer">Serializer</param>
        /// <returns>Domain model</returns>
        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer) =>
                reader == null
                ? throw new ArgumentNullException(nameof(existingValue))
                : new TDomainModel().Construct(reader.Value);

        /// <summary>
        /// Writes the domain model to JSON.
        /// </summary>
        /// <param name="writer">Write</param>
        /// <param name="value">Value to write</param>
        /// <param name="serializer">Serializer</param>
        public override void WriteJson
            (JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
            writer.Flush();
        }
    }
}
