using System;
using System.Collections.Generic;

namespace CustomerOrder.API.Contracts.CustomerOrders.Dtos
{
    public class CustomerOrderDto
    {
        public CustomerDto Customer { get; set; }
        //public int CustomerId { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
