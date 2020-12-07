using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class EfSqlProducts : ILookupProducts
    {
        public Task<GetProductDetailsResponse> GetProductById(int id)
        {
            return Task.FromResult(new GetProductDetailsResponse
            {
                Id = id,
                Name = "Some Product",
                Category = "Bread",
                Count = 1,
                Price = 1.89M
            });
        }
    }
}
