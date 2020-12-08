using ShoppingApi.Models.Products;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public interface IProductCommands
    {
        Task<GetProductDetailsResponse> Add(PostProductRequest productToAdd);
    }
}