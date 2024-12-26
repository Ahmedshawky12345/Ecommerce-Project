﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class PaginationDTO
    {
        
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
            public int PageNumber { get; set; } = 1;

            [Required]
            [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
            public int PageSize { get; set; } = 10;
        
    }
}