using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.CartItem
{
    public class CartItemDTO
    {
        [Required(ErrorMessage = "Cart ID is required.")]
        public int CartId { get; set; }
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        //[Required(ErrorMessage = "Quantity is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        //public int quntity { get; set; }
    }

    public class CartItemDetilesDto
    {
        public int ProductId { get; set; }
      
        public string ProductName { get; set; } = string.Empty;
        
        public int Quantity { get; set; }
        public string imageurl { get; set; }
       
        public decimal TotalPrice { get; set; }
        public int Stock { get; set; }
     
     
    }

}
