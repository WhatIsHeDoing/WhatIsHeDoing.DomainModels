using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatIsHeDoing.DomainModels.Locations;

namespace WhatIsHeDoing.DomainModels.Tests.Locations
{
    public class UkPostcodeTest
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void Valid()
            {
                var ukPostcode = new UKPostcode("BH13 6HB");

                Assert.AreEqual("BH13", ukPostcode.OutwardCode);
                Assert.AreEqual("BH", ukPostcode.PostcodeArea);
                Assert.AreEqual("13", ukPostcode.PostcodeDistrict);
                Assert.AreEqual("BH13 6", ukPostcode.PostcodeSector);
                Assert.AreEqual("6HB", ukPostcode.InwardCode);
                Assert.AreEqual("HB", ukPostcode.PostcodeUnit);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void Invalid()
            {
                new UKPostcode("oops");
            }
        }

        [TestClass]
        public class IsValid
        {
            [TestMethod]
            public void True()
            {
                Assert.IsTrue(UKPostcode.IsValid("GIR0AA"));
                Assert.IsTrue(UKPostcode.IsValid("GIR 0AA"));
            }

            [TestMethod]
            public void False()
            {
                Assert.IsFalse(UKPostcode.IsValid("oops"));
                Assert.IsFalse(UKPostcode.IsValid("GIR  0AA"));
            }
        }

        public class Equals
        {
            [TestMethod]
            public void True()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HB");

                Assert.AreEqual
                    (ukPostcodeOne, ukPostcodeTwo, "Postcodes identical");
            }

            [TestMethod]
            public void False()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HD");

                Assert.AreNotEqual
                    (ukPostcodeOne, ukPostcodeTwo);
            }
        }

        [TestMethod]
        public void ImplicitStringOperator()
        {
            const string expected = "BH13 6HB";
            var actual = (string)(new UKPostcode(expected));

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ToString()
        {
            const string expected = "BH13 6HB";
            var actual = new UKPostcode("BH13 6HB").ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestClass]
        public class GetHashCode
        {
            [TestMethod]
            public void Equal()
            {
                var ukPostcodeOne = new UKPostcode("BH136HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HB");

                Assert.AreEqual
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }

            [TestMethod]
            public void NotEqual()
            {
                var ukPostcodeOne = new UKPostcode("BH13 6HB");
                var ukPostcodeTwo = new UKPostcode("BH13 6HD");

                Assert.AreNotEqual
                    (ukPostcodeOne.GetHashCode(), ukPostcodeTwo.GetHashCode());
            }
        }
    }
}
