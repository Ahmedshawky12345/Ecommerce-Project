namespace Data.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Descrption { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageURL { get; set; }
        public DateTime createdate { get; set; }


        // rationship with Cartitem 
        public List<CartItem>? CartItems { get; set; }

        // rationship with orderitem 

        public ICollection<OrderItem>? orderItems { get; set; }

        // relationship with category
        public int? CategoryId { get;set; }
        public Category? category { get; set; }
        // relationship with copun
        public int? CoupnId { get; set; }
        public Coupon? Coupon { get; set; }

    }
}
