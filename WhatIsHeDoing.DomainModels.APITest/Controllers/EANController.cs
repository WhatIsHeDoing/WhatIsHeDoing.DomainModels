namespace WhatIsHeDoing.DomainModels.APITest.Controllers
{
    using Barcodes;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class EANController : Controller
    {
        [HttpGet]
        public EAN Get() => new EAN(73513537UL);

        [HttpGet("{EAN}")]
        public ulong Get(EAN EAN) => EAN.Value;

        [HttpPost]
        public ulong Post([FromBody]Product product) => product.EAN.Value;

        [HttpPut("{id}")]
        public Product Put(int id, [FromBody]Product product) => product;

        [HttpDelete("{EAN}")]
        public void Delete(EAN EAN) { }
    }
}
