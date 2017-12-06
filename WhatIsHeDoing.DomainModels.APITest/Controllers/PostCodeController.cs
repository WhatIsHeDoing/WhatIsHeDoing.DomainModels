namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Locations;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class PostCodeController : Controller
    {
        [HttpGet]
        public UKPostcode Get() => new UKPostcode("SW1 1AA");

        [HttpGet("{postcode}")]
        public string Get(UKPostcode postcode) => postcode.Value;

        [HttpPost]
        public string Post([FromBody]Address address) => address.Postcode.Value;

        [HttpPut("{id}")]
        public Address Put(int id, [FromBody]Address address) => address;

        [HttpDelete("{postcode}")]
        public void Delete(UKPostcode postcode) { }
    }
}
