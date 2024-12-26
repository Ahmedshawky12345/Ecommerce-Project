using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Cart
    {
        public int Id { get; set; }
        // relation with user
        public string? userid { get; set; }  
        public AppUser? AppUser { get; set; }
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
      
    }
}
