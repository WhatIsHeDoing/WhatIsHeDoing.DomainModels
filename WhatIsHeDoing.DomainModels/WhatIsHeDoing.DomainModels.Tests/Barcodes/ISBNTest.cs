using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatIsHeDoing.DomainModels.Barcodes;

namespace WhatIsHeDoing.DomainModels.Tests.Barcodes
{
    public static class ISBNTest
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void InvalidBarcode()
            {
                var ean = new ISBN("test");
            }
        }
    }
}
