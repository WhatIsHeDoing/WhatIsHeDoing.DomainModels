using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WhatIsHeDoing.DomainModels.Barcodes;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class ISBNTest
    {
        public class Book
        {
            public ISBN ISBN { get; set; }
            public string Title { get; set; }
        }

        public class Constructor
        {
            [Fact]
            public void Valid()
            {
                var barcode = new ISBN(9783161484100UL);
                Assert.NotNull(barcode);
            }

            [Fact]
            public void Invalid() => Assert.Throws<ArgumentException>
                (() => new ISBN(978316148410UL));
        }

        public class JSONSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var book = new Book
                {
                    ISBN = new ISBN(9783161484100UL),
                    Title = "Read Me"
                };

                var serialised = JsonConvert.SerializeObject(book);

                Assert.NotNull(serialised);
                Assert.Contains(@"""ISBN"":""9783161484100"",", serialised);
            }

            [Fact]
            public void CanDeserialise()
            {
                const string serialised = @"
{
    ""ISBN"": ""9783161484100"",
    ""Title"": ""Read Me""
}";

                var book = JsonConvert.DeserializeObject<Book>(serialised);
                Assert.NotNull(book);
                Assert.Equal("Read Me", book.Title);

                var barcode = book.ISBN;
                Assert.NotNull(barcode);
                Assert.Equal(9783161484100UL, barcode.Value);
            }

            [Fact]
            public void DeserialiseStringFail()
            {
                const string serialised = @"
{
    ""ISBN"": ""oops"",
    ""Title"": ""Read Me""
}";

                Assert.Throws<FormatException>
                    (() => JsonConvert.DeserializeObject<Book>(serialised));
            }

            [Fact]
            public void DeserialiseUnsignedLongFail()
            {
                const string serialised = @"
{
    ""ISBN"": ""12345"",
    ""Title"": ""Read Me""
}";

                Assert.Throws<ArgumentException>
                    (() => JsonConvert.DeserializeObject<Book>(serialised));
            }
        }

        public class XMLSerialisation
        {
            [Fact]
            public void CanSerialise()
            {
                var book = new Book
                {
                    ISBN = new ISBN(9783161484100UL),
                    Title = "Read Me"
                };

                var serialiser = new XmlSerializer(typeof(Book));

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, book);
                        var xml = stringWriter.ToString();
                        Assert.NotNull(xml);
                        Assert.Contains("<ISBN>9783161484100</ISBN>", xml);
                    }
                }
            }

            [Fact]
            public void CanDeserialise()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Book 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <ISBN>9783161484100</ISBN>
    <Title>Read Me</Title>
</Book>
";

                var deserializer = new XmlSerializer(typeof(Book));

                using (var reader = new StringReader(xml))
                {
                    var book = deserializer.Deserialize(reader) as Book;
                    Assert.NotNull(book);
                    Assert.Equal("Read Me", book.Title);

                    var barcode = book.ISBN;
                    Assert.NotNull(barcode);
                    Assert.Equal(9783161484100UL, barcode.Value);
                }
            }

            [Fact]
            public void DeserialiseStringFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Book 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <ISBN>oops</ISBN>
    <Title>Read Me</Title>
</Book>
";

                var deserializer = new XmlSerializer(typeof(Book));

                using (var reader = new StringReader(xml))
                {
                    Assert.Throws<InvalidOperationException>
                        (() => deserializer.Deserialize(reader));
                }
            }

            [Fact]
            public void DeserialiseUnsignedLongFail()
            {
                const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Book 
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <ISBN>123</ISBN>
    <Title>Read Me</Title>
</Book>
";

                var deserializer = new XmlSerializer(typeof(Book));

                using (var reader = new StringReader(xml))
                {
                    Assert.Throws<InvalidOperationException>
                        (() => deserializer.Deserialize(reader));
                }
            }
        }
    }
}
