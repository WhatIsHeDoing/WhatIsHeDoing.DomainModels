namespace WhatIsHeDoing.DomainModels.Tests.Locations
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using WhatIsHeDoing.DomainModels.Locations;
    using Xunit;

    public class UKPostcodeTest
    {
        [Fact]
        public void GetSchema() => Assert.Null(new UKPostcode("SW1 1AA").GetSchema());

        [Fact]
        public void ToStringTests()
        {
            const string expected = "SW1A 1AA";
            var actual = new UKPostcode("SW1A 1AA").ToString();
            Assert.Equal(expected, actual);
        }

        public class Address
        {
            public string Country { get; set; }
            public UKPostcode UKPostcode { get; set; }
        }

        public class Constructor
        {
            [Fact]
            public void ValidFiveCharacters()
            {
                var ukPostcode = new UKPostcode("S2 4SU");

                Assert.Equal("S2", ukPostcode.OutwardCode);
                Assert.Equal("S", ukPostcode.PostcodeArea);
                Assert.Equal("2", ukPostcode.PostcodeDistrict);
                Assert.Equal("S2 4", ukPostcode.PostcodeSector);
                Assert.Equal("4SU", ukPostcode.InwardCode);
                Assert.Equal("SU", ukPostcode.PostcodeUnit);
            }

            [Fact]
            public void ValidSixCharacters()
            {
                var ukPostcode = new UKPostcode("BX1 1LT");

                Assert.Equal("BX1", ukPostcode.OutwardCode);
                Assert.Equal("BX", ukPostcode.PostcodeArea);
                Assert.Equal("1", ukPostcode.PostcodeDistrict);
                Assert.Equal("BX1 1", ukPostcode.PostcodeSector);
                Assert.Equal("1LT", ukPostcode.InwardCode);
                Assert.Equal("LT", ukPostcode.PostcodeUnit);
            }

            [Fact]
            public void ValidSevenCharacters()
            {
                var ukPostcode = new UKPostcode("SW1A 1AA");

                Assert.Equal("SW1A", ukPostcode.OutwardCode);
                Assert.Equal("SW", ukPostcode.PostcodeArea);
                Assert.Equal("1A", ukPostcode.PostcodeDistrict);
                Assert.Equal("SW1A 1", ukPostcode.PostcodeSector);
                Assert.Equal("1AA", ukPostcode.InwardCode);
                Assert.Equal("AA", ukPostcode.PostcodeUnit);
            }

            [Fact]
            public void Invalid() =>
                Assert.Throws<DomainValueException>(() => new UKPostcode("oops"));
        }

        public class IsValid
        {
            [Theory]
            [InlineData("GIR0AA")]
            [InlineData("GIR 0AA")]
            [InlineData("GIR  0AA")]
            public void True(string postcode) =>
                Assert.True(UKPostcode.IsValid(postcode));

            [Theory]
            [InlineData("oops")]
            [InlineData("GIR 0A")]
            public void False(string postcode) =>
                Assert.False(UKPostcode.IsValid(postcode));
        }

        public class EqualsTests
        {
            [Fact]
            public void True()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A1AA");
                Assert.Equal(ukPostcodeOne, ukPostcodeTwo);
            }

            [Fact]
            public void False()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A 2AA");
                Assert.NotEqual(ukPostcodeOne, ukPostcodeTwo);
            }
        }

        public class ImplicitStringOperator
        {
            [Fact]
            public void Casting()
            {
                const string expected = "SW1A 1AA";
                var postcode = new UKPostcode(expected);
                var actual = (string)postcode;
                Assert.Equal(actual, expected);
            }

            [Fact]
            public void StringFormat()
            {
                var postcode = new UKPostcode("SW1 1AA");
                var actual = $"Hello from {postcode}!";
                Assert.Equal("Hello from SW1 1AA!", actual);
            }
        }

        public class GetHashCodeTests
        {
            [Fact]
            public void Equal()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A 1AA");
                Assert.Equal(ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }

            [Fact]
            public void NotEqual()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A 2AA");
                Assert.NotEqual(ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }
        }

        public class JSONSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var address = new Address
                {
                    Country = "England",
                    UKPostcode = new UKPostcode("SW1 1AA")
                };

                var serialised = JsonConvert.SerializeObject(address);

                Assert.NotNull(serialised);
                Assert.Contains(@"""UKPostcode"":""SW1 1AA""", serialised);
            }

            [Fact]
            public void CanDeserialise()
            {
                const string serialised = @"
{
    ""Country"": ""England"",
    ""UKPostcode"": ""SW1 1AA""
}";

                var deserialised = JsonConvert.DeserializeObject<Address>(serialised);

                Assert.NotNull(deserialised);
                Assert.Equal("England", deserialised.Country);

                var postcode = deserialised.UKPostcode;
                Assert.NotNull(postcode);
                Assert.Equal("SW1 1AA", postcode.Value);
                Assert.Equal("SW1", postcode.OutwardCode);
            }

            [Fact]
            public void DeserialiseFail()
            {
                const string serialised = @"
{
    ""Country"": ""England"",
    ""UKPostcode"": ""oops""
}";

                Assert.Throws<DomainValueException>(
                    () => JsonConvert.DeserializeObject<Address>(serialised));
            }
        }

        public class TryParse
        {
            [Fact]
            public void Success()
            {
                const string postcode = "SW1 1AA";
                Assert.True(UKPostcode.TryParse(postcode, out var model));
                Assert.Equal(postcode, model);
            }

            [Fact]
            public void BadValue()
            {
                Assert.False(UKPostcode.TryParse("oops", out var model));
                Assert.Null(model);
            }
        }

        public class XMLSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var address = new Address
                {
                    Country = "England",
                    UKPostcode = new UKPostcode("SW1A 1AA")
                };

                var serialiser = new XmlSerializer(typeof(Address));

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, address);
                        var xml = stringWriter.ToString();
                        Assert.NotNull(xml);
                        Assert.Contains("<UKPostcode>SW1A 1AA</UKPostcode>", xml);
                    }
                }
            }

            [Fact]
            public void CanDeserialise()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Address 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <Country>England</Country>
    <UKPostcode>SW1A 1AA</UKPostcode>
</Address>
";

                var deserializer = new XmlSerializer(typeof(Address));

                using (var reader = new StringReader(xml))
                {
                    var address = deserializer.Deserialize(reader) as Address;
                    Assert.NotNull(address);
                    Assert.Equal("England", address.Country);

                    var postcode = address.UKPostcode;
                    Assert.NotNull(postcode);
                    Assert.Equal("SW1A 1AA", postcode.Value);
                    Assert.Equal("SW1A", postcode.OutwardCode);
                }
            }

            [Fact]
            public void DeserialiseFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Address 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <Country>England</Country>
    <UKPostcode>oops</UKPostcode>
</Address>
";

                var deserializer = new XmlSerializer(typeof(Address));

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
