namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Barcodes;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;

    [Route("api/[controller]")]
    public class ISBNController : Controller
    {
        /// <summary>
        /// Gets an ISBN.
        /// </summary>
        /// <returns>ISBN</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        public IActionResult Get() => Ok(new ISBN(9783161484100UL));

        /// <summary>
        /// Gets the raw value of an ISBN.
        /// </summary>
        /// <param name="barcode">ISBN</param>
        /// <returns>Success</returns>
        [HttpGet("{barcode}")]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get(ISBN barcode) => ModelState.IsValid
            ? Ok(barcode.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Posts an ISBN from within a product model.
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Success</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody]Product product) => ModelState.IsValid
            ? Ok(product.ISBN.Value)
            : (ActionResult)new BadRequestResult();

        /// <summary>
        /// Puts a product and ensures the ISBN can be returned within the product.
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
        /// Deletes an ISBN.
        /// </summary>
        /// <param name="barcode">ISBN</param>
        /// <returns>Success</returns>
        [HttpDelete("{barcode}")]
        [ProducesResponseType(typeof(ulong), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(ISBN barcode) => ModelState.IsValid
            ? Ok()
            : (ActionResult)new BadRequestResult();
    }
}
