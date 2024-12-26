using Data.DTO.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class CartDTO
    {
        public decimal SubTotal { get; set; }
        public int ItemCount { get; set; }
        public List<CartItemDetilesDto> Items { get; set; }
    }
}
