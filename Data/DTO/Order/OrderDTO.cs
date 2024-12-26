using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.Order
{
    public class OrderDTO
    {
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Amount")]
        [Required(ErrorMessage = "Total amount is required.")]
        public decimal TotalAmount { get; set; }

    }

}
