using Newtonsoft.Json;
using System;
using WhatIsHeDoing.DomainModels.Barcodes;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class EANTest
    {
        public class Constructor
        {
            [Theory]
            [InlineData(73513537UL)]
            [InlineData(4006381333931UL)]
            public void Valid(ulong EAN) =>
                Assert.Equal(EAN, new EAN(EAN).Value);

            [Fact]
            public void InvalidBarcode() =>
                Assert.Throws<ArgumentException>(() => new EAN(7351353UL));
        }

        public class Serialisation
        {
            public class Product
            {
                public string Name { get; set; }

                [JsonConverter(typeof(DomainModelConverter<EAN, ulong>))]
                public EAN EAN { get; set; }
            }

            [Fact]
            public void SerialiseAndDeserialise()
            {
                var address = new Product
                {
                    Name = "Yummy Food",
                    EAN = new EAN(4006381333931UL)
                };

                var serialised = JsonConvert.SerializeObject(address);
                var deserialised = JsonConvert.DeserializeObject<Product>(serialised);

                Assert.NotNull(deserialised);
                Assert.Equal(address.Name, deserialised.Name);
                Assert.Equal(address.EAN, deserialised.EAN);
            }

            [Fact]
            public void DeserialiseStringFail()
            {
                const string serialised = @"
{
    ""Name"": ""Yummy Food"",
    ""EAN"": ""oops""
}";

                Assert.Throws<FormatException>
                    (() => JsonConvert.DeserializeObject<Product>(serialised));
            }

            [Fact]
            public void DeserialiseLongFail()
            {
                const string serialised = @"
{
    ""Name"": ""Yummy Food"",
    ""EAN"": ""1234""
}";

                Assert.Throws<ArgumentException>
                    (() => JsonConvert.DeserializeObject<Product>(serialised));
            }
        }
    }
}
