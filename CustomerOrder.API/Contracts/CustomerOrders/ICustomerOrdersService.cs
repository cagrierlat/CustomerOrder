using CustomerOrder.API.Contracts.CustomerOrders.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerOrder.API.Contracts.CustomerOrders
{
    public interface ICustomerOrdersService
    {
        Task<CustomerOrderDto> AddCustomerOrderAsync(CustomerOrderDto orderDto);
        Task<CustomerOrderDto> GetByIdAsync(int id);
        Task<CustomerOrderDto> UpdateAsync(int Id, CustomerOrderDto orderDto);
        Task<bool> DeleteAsync(int id);
    }
}
