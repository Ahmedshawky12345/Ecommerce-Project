using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class CartItem
    {
     
        public int? CartId { get; set; }
        public Cart? Cart { get; set; }

        public int? ProId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal TotalPrice { get; set; }
    }
}
