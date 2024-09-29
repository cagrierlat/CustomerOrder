using CustomerOrder.API.Contracts.CustomerOrders;
using CustomerOrder.API.Contracts.CustomerOrders.Dtos;
using CustomerOrder.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace CustomerOrder.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly ICustomerOrdersService _customerOrderService;
        public CustomerOrdersController(ICustomerOrdersService customerOrderService)
        {
            _customerOrderService = customerOrderService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<CustomerOrderDto>> Add(CustomerOrderDto orderDto)
        {
            var result = await _customerOrderService.AddCustomerOrderAsync(orderDto);
            //return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<CustomerOrderDto>> Get(int id)
        {
            var order = await _customerOrderService.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost("update")]
        public async Task<ActionResult<CustomerOrderDto>> Update(int Id, CustomerOrderDto orderDto)
        {
            var updatedOrder = await _customerOrderService.UpdateAsync(Id, orderDto);
            return Ok(updatedOrder);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerOrderService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
