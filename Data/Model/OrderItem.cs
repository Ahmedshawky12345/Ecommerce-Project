using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class OrderItem
    {
       
         
        public int Quantity { get; set; }  
        public decimal UnitPrice { get; set; }  
        public decimal TotalPrice { get; set; }

        //relationship with product
        public Product? product { get;set; }
        public int? ProductId { get;set; }
        // relationship with order

        public Order? Order { get; set; }
        public int? OrderId { get;set; }

    }
}
