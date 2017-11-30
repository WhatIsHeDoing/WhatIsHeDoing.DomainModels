using Newtonsoft.Json;
using System;
using WhatIsHeDoing.DomainModels.Locations;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Locations
{
    public class UkPostcodeTest
    {
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
            public void ValidOne()
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
                Assert.Throws<ArgumentException>(() => new UKPostcode("oops"));
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

        [Fact]
        public void ImplicitStringOperator()
        {
            const string expected = "SW1A 1AA";
            var postcode = new UKPostcode(expected);
            var actual = (string)postcode;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ToStringTests()
        {
            const string expected = "SW1A 1AA";
            var actual = new UKPostcode("SW1A 1AA").ToString();
            Assert.Equal(expected, actual);
        }

        public class GetHashCodeTests
        {
            [Fact]
            public void Equal()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A 1AA");

                Assert.Equal
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }

            [Fact]
            public void NotEqual()
            {
                var ukPostcodeOne = new UKPostcode("SW1A 1AA");
                var ukPostcodeTwo = new UKPostcode("SW1A 2AA");

                Assert.NotEqual
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }
        }

        public class Serialisation
        {
            public class Address
            {
                public string Country { get; set; }

                [JsonConverter(typeof(DomainModelConverter<UKPostcode, string>))]
                public UKPostcode UKPostcode { get; set; }
            }

            [Fact]
            public void SerialiseAndDeserialise()
            {
                var address = new Address
                {
                    Country = "England",
                    UKPostcode = new UKPostcode("SW1 1AA")
                };

                var serialised = JsonConvert.SerializeObject(address);
                var deserialised = JsonConvert.DeserializeObject<Address>(serialised);

                Assert.NotNull(deserialised);
                Assert.Equal(address.Country, deserialised.Country);
                Assert.Equal(address.UKPostcode, deserialised.UKPostcode);
            }

            [Fact]
            public void DeserialiseFail()
            {
                const string serialised = @"
{
    ""Country"": ""England"",
    ""UkPostcode"": ""oops""
}";

                Assert.Throws<ArgumentException>
                    (() => JsonConvert.DeserializeObject<Address>(serialised));
            }
        }
    }
}
