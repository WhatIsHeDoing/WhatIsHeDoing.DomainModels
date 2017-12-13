namespace WhatIsHeDoing.DomainModels
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class DomainModelTypeConverter<TDomainModel, TValue> : TypeConverter
        where TDomainModel : IDomainModel<TValue>, new()
    {
        // Always assume the value can be used.
        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType) => true;

        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value) =>
                new TDomainModel().Construct(value);
    }
}
