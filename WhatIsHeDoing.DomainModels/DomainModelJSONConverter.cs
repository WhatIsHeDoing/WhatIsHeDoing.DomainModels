namespace WhatIsHeDoing.DomainModels
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Converts any JSON object to a domain model.
    /// </summary>
    /// <typeparam name="TDomainModel">Domain model type</typeparam>
    /// <typeparam name="TValue">Domain model value</typeparam>
    public class DomainModelJSONConverter<TDomainModel, TValue> :
        JsonConverter where TDomainModel : IDomainModel<TValue>, new()
    {
        /// <remarks>Always assume the value can be used.</remarks>
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer) =>
                reader == null
                ? throw new ArgumentNullException(nameof(existingValue))
                : new TDomainModel().Construct(reader.Value);

        public override void WriteJson
            (JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
            writer.Flush();
        }
    }
}
