﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.OrderItem
{
    public class updateOrderitemDTO
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
    }
}
