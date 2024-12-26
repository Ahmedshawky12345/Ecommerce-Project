using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
  public  class productwithCategoryDTO
    {
        public int productId { get; set; }
        public string? productName { get; set; }
         public int categoryId { get; set; }
        public string? categoryName { get; set;}
    }
}
