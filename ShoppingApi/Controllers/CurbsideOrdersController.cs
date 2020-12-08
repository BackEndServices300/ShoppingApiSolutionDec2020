using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Data;
using ShoppingApi.Models.CurbsideOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {
        private readonly ShoppingDataContext _context;

        public CurbsideOrdersController(ShoppingDataContext context)
        {
            _context = context;
        }

        [HttpPost("sync/curbsideorders")]
        public async Task<ActionResult> SyncCurbsideOrders([FromBody] PostSyncCurbsideOrdersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // Save it to the DB
                var orderToSave = new CurbsideOrder
                {
                    For = request.For,
                    Items = request.Items,
                };
                var numberOfItems = orderToSave.Items.Split(',').Count();
                for(var t = 0; t< numberOfItems; t++)
                {
                    await Task.Delay(1000);
                }
                orderToSave.PickupDate = DateTime.Now.AddHours(numberOfItems);
                // return 201, Location Header, Copy of the Entity
                _context.CurbsideOrders.Add(orderToSave);
                await _context.SaveChangesAsync();

                var response = new GetCurbsideOrderResponse
                {
                    Id = orderToSave.Id,
                    For = orderToSave.For,
                    Items = orderToSave.Items,
                    PickupDate = orderToSave.PickupDate.Value
                };
                return Ok(response);
            }
        }
    }
}
