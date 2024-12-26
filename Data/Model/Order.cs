using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Order
    {
        public int Id { get; set; }  
        public DateTime OrderDate { get; set; } = DateTime.Now;  
        public decimal TotalAmount { get; set; }
      
       
        // rationship with orderitem 

        public ICollection<OrderItem>? orderItems { get; set; }   

       // relationship with user 
       public string? UserId { get; set; }
        public AppUser? user { get; set; }

    }
}
