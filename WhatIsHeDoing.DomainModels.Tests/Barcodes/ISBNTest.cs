using Newtonsoft.Json;
using System;
using WhatIsHeDoing.DomainModels.Barcodes;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class ISBNTest
    {
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

        public class Serialisation
        {
            public class Book
            {
                [JsonConverter(typeof(DomainModelConverter<ISBN, ulong>))]
                public ISBN ISBN { get; set; }

                public string Title { get; set; }
            }

            [Fact]
            public void SerialiseAndDeserialise()
            {
                var book = new Book
                {
                    ISBN = new ISBN(9783161484100UL),
                    Title = "Read Me"
                };

                var serialised = JsonConvert.SerializeObject(book);
                var deserialised = JsonConvert.DeserializeObject<Book>(serialised);

                Assert.NotNull(deserialised);
                Assert.Equal(book.ISBN, deserialised.ISBN);
                Assert.Equal(book.Title, deserialised.Title);
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
            public void DeserialiseLongFail()
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
    }
}
