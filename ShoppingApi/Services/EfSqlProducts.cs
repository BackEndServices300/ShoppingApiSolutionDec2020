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
    public class EfSqlProducts : ILookupProducts
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
