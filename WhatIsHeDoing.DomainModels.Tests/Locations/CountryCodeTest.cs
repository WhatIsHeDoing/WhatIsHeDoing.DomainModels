using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WhatIsHeDoing.DomainModels.Locations;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Locations
{
    public class CountryCodeTest
    {
        public class Country
        {
            [JsonConverter(typeof(DomainModelConverter<CountryCode, string>))]
            public CountryCode CountryCode { get; set; }

            public string Name { get; set; }
        }

        public class IsValid
        {
            [Theory]
            [InlineData("gb")]
            [InlineData("GB")]
            [InlineData("ZWE")]
            public void Success(string isoCode) =>
                Assert.True(CountryCode.IsValid(isoCode));

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("A")]
            [InlineData("OOPS")]
            [InlineData("A1A")]
            [InlineData("A!A")]
            [InlineData("AAA!")]
            [InlineData("123")]
            public void Fail(string isoCode) =>
                Assert.False(CountryCode.IsValid(isoCode));
        }

        [Fact]
        public void ValidModelUsedAsString()
        {
            var isoCode = new CountryCode("gb");
            Assert.Equal("GB", isoCode.ToString());
            Assert.Equal("GB", isoCode + "");
        }

        public class JSONSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var country = new Country
                {
                    CountryCode = new CountryCode("GB"),
                    Name = "Great Britain"
                };

                var serialised = JsonConvert.SerializeObject(country);

                Assert.NotNull(serialised);
                Assert.Contains(@"""CountryCode"":""GB""", serialised);
            }

            [Fact]
            public void CanDeserialise()
            {
                const string serialised = @"
{
    ""CountryCode"": ""GB"",
    ""Name"": ""Great Britain""
}";

                var deserialised = JsonConvert.DeserializeObject<Country>(serialised);

                Assert.NotNull(deserialised);
                Assert.Equal("Great Britain", deserialised.Name);

                var countryCode = deserialised.CountryCode;
                Assert.NotNull(countryCode);
                Assert.Equal("GB", countryCode.Value);
            }

            [Fact]
            public void DeserialiseFail()
            {
                const string serialised = @"
{
    ""CountryCode"": ""oops"",
    ""Name"": ""Great Britain""
}";

                Assert.Throws<ArgumentException>
                    (() => JsonConvert.DeserializeObject<Country>(serialised));
            }
        }

        public class XMLSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var country = new Country
                {
                    CountryCode = new CountryCode("GB"),
                    Name = "Great Britain"
                };

                var serialiser = new XmlSerializer(typeof(Country));

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, country);
                        var xml = stringWriter.ToString();
                        Assert.NotNull(xml);
                        Assert.Contains("<CountryCode>GB</CountryCode>", xml);
                    }
                }
            }

            [Fact]
            public void CanDeserialise()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Country 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <CountryCode>GB</CountryCode>,
    <Name>Great Britain</Name>
</Country>
";

                var deserializer = new XmlSerializer(typeof(Country));

                using (var reader = new StringReader(xml))
                {
                    var country = deserializer.Deserialize(reader) as Country;
                    Assert.NotNull(country);
                    Assert.Equal("Great Britain", country.Name);

                    var countryCode = country.CountryCode;
                    Assert.NotNull(countryCode);
                    Assert.Equal("GB", countryCode.Value);
                }
            }

            [Fact]
            public void DeserialiseFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Country 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <CountryCode>oops</CountryCode>
    <Name>Great Britain</Name>
</Country>
";

                var deserializer = new XmlSerializer(typeof(Country));

                using (var reader = new StringReader(xml))
                {
                    Assert.Throws<InvalidOperationException>
                        (() => deserializer.Deserialize(reader));
                }
            }
        }
    }
}
