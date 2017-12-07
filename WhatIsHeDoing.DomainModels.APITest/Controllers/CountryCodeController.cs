namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Locations;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;

    [Route("api/[controller]")]
    public class CountryCodeController : Controller
    {
        /// <summary>
        /// Gets a country code.
        /// </summary>
        /// <returns>Country code</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Get() => Ok(new CountryCode("GB"));

        /// <summary>
        /// Gets the raw value of a country code.
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <returns>Success</returns>
        [HttpGet("{countryCode}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get(CountryCode countryCode) => ModelState.IsValid
            ? Ok(countryCode.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Posts a country code from within an address model.
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Success</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody]Address address) => ModelState.IsValid
            ? Ok(address.CountryCode.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Puts an address and ensures the country code can be returned within the address.
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="address">Address</param>
        /// <returns>Success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Address), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Put(int id, [FromBody]Address address) => ModelState.IsValid
            ? Ok(address)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Deletes a country code.
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <returns>Success</returns>
        [HttpDelete("{countryCode}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(CountryCode countryCode) => ModelState.IsValid
            ? Ok()
            : (ActionResult)new BadRequestResult();
    }
}
