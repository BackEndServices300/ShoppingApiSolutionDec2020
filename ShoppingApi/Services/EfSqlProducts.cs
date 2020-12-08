using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace ShoppingApi.Services
{
    public class EfSqlProducts : ILookupProducts, IProductCommands
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public EfSqlProducts(ShoppingDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        public async Task<GetProductDetailsResponse> Add(PostProductRequest productToAdd)
        {
            // we have a PostProductRequest (which we assume to be valid at this point)
            var product = _mapper.Map<Product>(productToAdd);
            // deal the category...
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name == productToAdd.Category);
            if(category == null)
            {
                // do the upsert.
                var newCategory = new Category { Name = productToAdd.Category };
                _context.Categories.Add(newCategory);
                product.Category = newCategory;
            } else
            {
                product.Category = category;
            }
            // take the cost, apply the markup, and make that the price.
            product.Price = productToAdd.Cost.Value * 1.10M; // TODO: We want this "configurable"
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // We have to map it to a Product (that's what we can add to the _context.Products
            // And THEN we have to map that to a GetProductDetailsResponse because that's what we have to return.
            return _mapper.Map<GetProductDetailsResponse>(product);
        }

        public async Task<GetProductDetailsResponse> GetProductById(int id)
        {
            var response = await _context.Products
                .Where(p => p.Id == id && p.RemovedFromInventory == false)
                .ProjectTo<GetProductDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();
            return response;
        }
    }
}
