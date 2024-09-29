using System;

namespace CustomerOrder.API.Contracts.CustomerOrders.Dtos
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
