namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Barcodes;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class ISBNController : Controller
    {
        [HttpGet]
        public ISBN Get() => new ISBN(9783161484100UL);

        [HttpGet("{ISBN}")]
        public ulong Get(ISBN ISBN) => ISBN.Value;

        [HttpPost]
        public ulong Post([FromBody]Product product) => product.ISBN.Value;

        [HttpPut("{id}")]
        public Product Put(int id, [FromBody]Product product) => product;

        [HttpDelete("{ISBN}")]
        public void Delete(ISBN ISBN) { }
    }
}
