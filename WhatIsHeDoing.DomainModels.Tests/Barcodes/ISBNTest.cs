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
            public void InvalidBarcode() =>
                Assert.Throws<ArgumentException>(() => new ISBN("test"));
        }
    }
}
