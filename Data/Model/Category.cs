using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public  class Category
    {
       
            public int Id { get; set; }
            public string Name { get; set; }

        // relationship with porduct
        public ICollection<Product>? products { get; set; }


        
    }
}
