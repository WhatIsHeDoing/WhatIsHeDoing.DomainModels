namespace WhatIsHeDoing.DomainModels.Models
{
    using Locations;

    public class Address
    {
        public CountryCode CountryCode { get; set; }
        public UKPostcode Postcode { get; set; }
    }
}
