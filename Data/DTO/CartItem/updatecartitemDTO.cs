using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.CartItem
{
    public class updatecartitemDTO
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int productid { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int quntity { get; set; }

    }
}
