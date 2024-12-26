using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class UpdateProductDTO
    {
      
        public int? Product_Id { get; set; }
       
        public string? Product_Name { get; set; }
      
        public decimal price { get; set; }
        
        public int? Stock { get; set; }
         public IFormFile? image { get; set; }
    }
}
