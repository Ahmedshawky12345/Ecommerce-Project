using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
  public  class CategoryDTO
    {
        [Required(ErrorMessage ="Id is requird to update category")]
        
        public int CategoryID { get;set; }
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters long.")]
        [MaxLength(1000, ErrorMessage = "Category name cannot exceed 1000 characters.")]
        public string? CategoryName { get;set; } 
        
    }
    public class AddCategoryDTO
    {
        [Required(ErrorMessage = " Category name Required")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters long.")]
        [MaxLength(1000, ErrorMessage = "Category name cannot exceed 1000 characters.")]
        public string CategoryName { get; set; }


    }
}
