using CustomerOrder.API.Contracts.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerOrder.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
