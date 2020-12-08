using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Models.Products;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly ILookupProducts _productLookup;
        private readonly IProductCommands _productCommands;

        public ProductsController(ILookupProducts productLookup, IProductCommands productCommands)
        {
            _productLookup = productLookup;
            _productCommands = productCommands;
        }

        [HttpPost("/products")]
        public async Task<ActionResult> AddAProduct([FromBody] PostProductRequest productToAdd)
        {

           


            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } else
            {
               
                // Add it to the domain.
                GetProductDetailsResponse response = await _productCommands.Add(productToAdd);
                // return:
                // 201 Created
                // Location header with the URL of the new thingy.
                // And a copy of what they would get if they followed that URL.
                return Ok(productToAdd); // TODO: Make the for realz.
            }
        }

        [HttpGet("/products/{id:int}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            GetProductDetailsResponse response = await _productLookup.GetProductById(id);
  
            return this.Maybe(response);
        }
    }
}
