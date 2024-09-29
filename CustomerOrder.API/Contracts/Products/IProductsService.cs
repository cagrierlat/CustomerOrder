using CustomerOrder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerOrder.API.Contracts.Products
{
    public interface IProductsService
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}
