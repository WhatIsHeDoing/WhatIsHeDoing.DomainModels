using System;
using WhatIsHeDoing.DomainModels.Barcodes;
using Xunit;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class EANTest
    {
        public class Constructor
        {
            [Fact]
            public void InvalidBarcode() =>
                Assert.Throws<ArgumentException>(() => new EAN("test"));
        }
    }
}
