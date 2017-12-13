namespace WhatIsHeDoing.DomainModels.Tests.Locations
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using WhatIsHeDoing.DomainModels.Locations;
    using Xunit;

    public class CountryCodeTest
    {
        [Fact]
        public void ValidModelUsedAsString()
        {
            var isoCode = new CountryCode("gb");
            Assert.Equal("GB", isoCode.ToString());
            Assert.Equal("GB", isoCode + string.Empty);
        }

        public class Country
        {
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

                Assert.Throws<DomainValueException>(
                    () => JsonConvert.DeserializeObject<Country>(serialised));
            }
        }

        public class TryParse
        {
            [Fact]
            public void Success()
            {
                const string countryCode = "US";
                Assert.True(CountryCode.TryParse(countryCode, out var model));
                Assert.Equal(countryCode, model);
            }

            [Fact]
            public void BadValue()
            {
                Assert.False(CountryCode.TryParse("oops", out var model));
                Assert.Null(model);
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
                    try
                    {
                        deserializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Assert.IsType<DomainValueException>(ex.InnerException);
                    }
                }
            }
        }
    }
}
