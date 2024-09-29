using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOrder.Data.Models
{
    public class Product : EntityBase
    {
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("CustomerOrder")]
        public int CustomerOrderId { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
    }
}
