using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult GetHomeDoc()
        {
            var doc = new Dictionary<string, Link>()
            {
                { "self", new Link { Href="http://localhost:1337/"} },
                { "store:products", new Link { Href="http://localhost:1337/products"} },
                { "store:curbside", new Link { Href="http://anotherserver/curbsideorders"} }
            };
            return Ok(doc);
        }
    }

    public class Link
    {
        public string Href { get; set; }
        
    }
}
