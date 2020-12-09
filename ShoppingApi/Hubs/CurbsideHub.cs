using Microsoft.AspNetCore.SignalR;
using ShoppingApi.Data;
using ShoppingApi.Models.CurbsideOrders;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Hubs
{
    public class CurbsideHub : Hub
    {
        private readonly ShoppingDataContext _context;
        private readonly CurbsideChannel _channel;

        public CurbsideHub(ShoppingDataContext context, CurbsideChannel channel)
        {
            _context = context;
            _channel = channel;
        }
        public async Task PlaceOrder(PostSyncCurbsideOrdersRequest request)
        {
            var orderToSave = new CurbsideOrder
            {
                For = request.For,
                Items = request.Items,
                Status = CurbsideOrderStatus.Processing
            };

            

            _context.CurbsideOrders.Add(orderToSave);

            await _context.SaveChangesAsync();

            var response = new GetCurbsideOrderResponse
            {
                Id = orderToSave.Id,
                For = orderToSave.For,
                Items = orderToSave.Items,
                PickupDate = null,
                Status = orderToSave.Status
            };
            var didWrite = await _channel.AddCurbside(new CurbsideChannelRequest { ReservationId = response.Id, ConnectionId=Context.ConnectionId });
            if (!didWrite)
            {
                // what to do?
            }
            await Clients.Caller.SendAsync("OrderPlaced", response);
        }
    }
}
