namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Barcodes;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;

    [Route("api/[controller]")]
    public class EANController : Controller
    {
        /// <summary>
        /// Gets an EAN.
        /// </summary>
        /// <returns>EAN</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        public IActionResult Get() => Ok(new EAN(73513537UL));

        /// <summary>
        /// Gets the raw value of an EAN.
        /// </summary>
        /// <param name="EAN">EAN</param>
        /// <returns>Success</returns>
        [HttpGet("{EAN}")]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get(EAN EAN) => ModelState.IsValid
            ? Ok(EAN.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Posts an EAN from within a product model.
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Success</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody]Product product) => ModelState.IsValid
            ? Ok(product.EAN.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Puts a product and ensures the EAN can be returned within the product.
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="product">Product</param>
        /// <returns>Success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Put(int id, [FromBody]Product product) => ModelState.IsValid
            ? Ok(product)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Deletes an EAN.
        /// </summary>
        /// <param name="EAN">EAN</param>
        /// <returns>Success</returns>
        [HttpDelete("{EAN}")]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(EAN EAN) => ModelState.IsValid
            ? Ok()
            : (ActionResult)new BadRequestResult();
    }
}
