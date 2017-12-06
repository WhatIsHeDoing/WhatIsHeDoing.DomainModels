namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Locations;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class CountryCodeController : Controller
    {
        [HttpGet]
        public CountryCode Get() => new CountryCode("GB");

        [HttpGet("{countryCode}")]
        public string Get(CountryCode countryCode) => countryCode.Value;

        [HttpPost]
        public string Post([FromBody]Address address) => address.CountryCode.Value;

        [HttpPut("{id}")]
        public Address Put(int id, [FromBody]Address address) => address;

        [HttpDelete("{countryCode}")]
        public void Delete(CountryCode countryCode) { }
    }
}
