namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Locations;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;

    [Route("api/[controller]")]
    public class PostCodeController : Controller
    {
        /// <summary>
        /// Gets a postcode.
        /// </summary>
        /// <returns>Postcode</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Get() => Ok(new UKPostcode("SW1 1AA"));

        /// <summary>
        /// Gets the raw value of a postcode.
        /// </summary>
        /// <param name="postcode">Postcode</param>
        /// <returns>Success</returns>
        [HttpGet("{postcode}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get(UKPostcode postcode) => ModelState.IsValid
            ? Ok(postcode.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Posts a postcode from within an address model.
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Success</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody]Address address) => ModelState.IsValid
            ? Ok(address.Postcode.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Puts an address and ensures the postcode can be returned within the address.
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
        /// Deletes a postcode.
        /// </summary>
        /// <param name="postcode">Postcode</param>
        /// <returns>Success</returns>
        [HttpDelete("{postcode}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(UKPostcode postcode) => ModelState.IsValid
            ? Ok()
            : (ActionResult)new BadRequestResult();
    }
}
