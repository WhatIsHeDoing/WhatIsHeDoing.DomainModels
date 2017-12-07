using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WhatIsHeDoing.DomainModels.Barcodes;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class EANTest
    {
        public class Product
        {
            public string Name { get; set; }
            public EAN EAN { get; set; }
        }

        public class Constructor
        {
            [Theory]
            [InlineData(73513537UL)]
            [InlineData(4006381333931UL)]
            public void Valid(ulong EAN) =>
                Assert.Equal(EAN, new EAN(EAN).Value);

            [Fact]
            public void InvalidBarcode() =>
                Assert.Throws<DomainValueException>(() => new EAN(7351353UL));
        }

        public class JSONSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var product = new Product
                {
                    Name = "Yummy Food",
                    EAN = new EAN(4006381333931UL)
                };

                var serialised = JsonConvert.SerializeObject(product);
                Assert.NotNull(serialised);
                Assert.Contains(@"""EAN"":4006381333931", serialised);
            }

            [Fact]
            public void CanDeserialise()
            {
                const string serialised = @"
{
    ""Name"": ""Yummy Food"",
    ""EAN"": ""4006381333931""
}";

                var product = JsonConvert.DeserializeObject<Product>(serialised);
                Assert.NotNull(product);
                Assert.Equal("Yummy Food", product.Name);

                var barcode = product.EAN;
                Assert.NotNull(barcode);
                Assert.Equal(4006381333931UL, barcode.Value);
            }

            [Fact]
            public void DeserialiseStringFail()
            {
                const string serialised = @"
{
    ""Name"": ""Yummy Food"",
    ""EAN"": ""oops""
}";

                Assert.Throws<DomainValueException>
                    (() => JsonConvert.DeserializeObject<Product>(serialised));
            }

            [Fact]
            public void DeserialiseUnsignedLongFail()
            {
                const string serialised = @"
{
    ""Name"": ""Yummy Food"",
    ""EAN"": ""1234""
}";

                Assert.Throws<DomainValueException>
                    (() => JsonConvert.DeserializeObject<Product>(serialised));
            }
        }

        public class XMLSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var product = new Product
                {
                    Name = "Yummy Food",
                    EAN = new EAN(4006381333931UL)
                };

                var serialiser = new XmlSerializer(typeof(Product));

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, product);
                        var xml = stringWriter.ToString();
                        Assert.NotNull(xml);
                        Assert.Contains("<EAN>4006381333931</EAN>", xml);
                    }
                }
            }

            [Fact]
            public void CanDeserialise()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Product 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <EAN>4006381333931</EAN>
    <Name>Yummy Food</Name>
</Product>
";

                var deserializer = new XmlSerializer(typeof(Product));

                using (var reader = new StringReader(xml))
                {
                    var product = deserializer.Deserialize(reader) as Product;
                    Assert.NotNull(product);
                    Assert.Equal("Yummy Food", product.Name);

                    var barcode = product.EAN;
                    Assert.NotNull(barcode);
                    Assert.Equal(4006381333931UL, barcode.Value);
                }
            }

            [Fact]
            public void DeserialiseStringFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Product 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <EAN>oops</EAN>
    <Name>Yummy Food</Name>
</Product>
";

                var deserializer = new XmlSerializer(typeof(Product));

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

            [Fact]
            public void DeserialiseUnsignedLongFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Product 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <EAN>123</EAN>
    <Name>Yummy Food</Name>
</Product>
";

                var deserializer = new XmlSerializer(typeof(Product));

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
