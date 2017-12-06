namespace WhatIsHeDoing.DomainModels.Models
{
    using Barcodes;

    public class Product
    {
        public EAN EAN { get; set; }
        public ISBN ISBN { get; set; }
    }
}
