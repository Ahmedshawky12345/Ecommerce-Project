using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.Order
{
    public class OrderDetitlesDTO
    {
        public int orderid { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public List<orderitemdata> orderitemdatas { get; set; }

    }
    public class orderitemdata
    {
        public int ProductId { get; set; }
        public string productname { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string StripeCustomerId { get; set; }



    }
}
