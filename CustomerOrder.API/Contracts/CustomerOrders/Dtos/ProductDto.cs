using System;

namespace CustomerOrder.API.Contracts.CustomerOrders.Dtos
{
    public class ProductDto
    {
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
