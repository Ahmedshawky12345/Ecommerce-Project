using Microsoft.AspNetCore.Identity;

namespace Data.Model
{
    public class AppUser:IdentityUser
    {
        //public string? StripeCustomerId { get; set; }
        public string Address { get; set; }
        // relation with cart
        public int? CartId { get; set; }
        public Cart? cart { get; set; }
        // relationship with Order
        public ICollection<Order>? orders { get; set; }
    }
}
