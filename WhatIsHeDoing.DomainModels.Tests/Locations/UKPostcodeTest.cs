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
            public void Valid()
            {
                var ukPostcode = new UKPostcode("BH13 6HB");

                Assert.Equal("BH13", ukPostcode.OutwardCode);
                Assert.Equal("BH", ukPostcode.PostcodeArea);
                Assert.Equal("13", ukPostcode.PostcodeDistrict);
                Assert.Equal("BH13 6", ukPostcode.PostcodeSector);
                Assert.Equal("6HB", ukPostcode.InwardCode);
                Assert.Equal("HB", ukPostcode.PostcodeUnit);
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
            public void True(string postcode) =>
                Assert.True(UKPostcode.IsValid(postcode));

            [Theory]
            [InlineData("oops")]
            [InlineData("GIR  0AA")]
            public void False(string postcode) =>
                Assert.False(UKPostcode.IsValid(postcode));
        }

        public class EqualsTests
        {
            [Fact]
            public void True()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HB");

                Assert.Equal
                    (ukPostcodeOne, ukPostcodeTwo);
            }

            [Fact]
            public void False()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HD");

                Assert.NotEqual
                    (ukPostcodeOne, ukPostcodeTwo);
            }
        }

        [Fact]
        public void ImplicitStringOperator()
        {
            const string expected = "BH13 6HB";
            var postcode = new UKPostcode(expected);
            var actual = (string)postcode;

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ToStringTests()
        {
            const string expected = "BH13 6HB";
            var actual = new UKPostcode("BH13 6HB").ToString();

            Assert.Equal(expected, actual);
        }

        public class GetHashCodeTests
        {
            [Fact]
            public void Equal()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HB");

                Assert.Equal
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }

            [Fact]
            public void NotEqual()
            {
                var ukPostcodeOne = new UKPostcode("BH13 6HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HD");

                Assert.NotEqual
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }
        }
    }
}
