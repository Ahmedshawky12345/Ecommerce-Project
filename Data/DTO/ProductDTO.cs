
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Data.DTO
{
   public class ProductDTO
    {
      
        public int Product_Id { get; set; }
     
        public string Product_Name { get; set; }
        public string? Descrption { get; set; }

        public decimal price { get; set; }
        
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public string ImageURL { get; set; }
       
        public int couponid { get; set; }
    }
    public class ProductAddDTO
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Product_Name { get; set; }
        [Required(ErrorMessage = "Product Descrption is required.")]
        //[StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string? Descrption { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal price { get; set; }
        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
       
        public int Stock { get; set; }

        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Image file is required.")]
        public IFormFile  image { get; set; }

    }
    public class prodcutHascouponDTO
    {

        public int Product_Id { get; set; }

        public string Product_Name { get; set; }
        public string? Descrption { get; set; }

        public decimal price { get; set; }

        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public string ImageURL { get; set; }
        public int discount { get; set; }
        public int couponid { get; set; }
    }

}
