using AutoMapper;

using Data.Model;
using Data.DTO.User;
using Data.DTO;
using Data.DTO.CartItem;
using Data.DTO.OrderItem;
using Data.DTO.Order;
using Data.DTO.Coupons;

namespace onlineShopping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // -----------------------------user----------------------------------
            CreateMap<RegsiterDTO, AppUser>();
            CreateMap<LoginDTO, AppUser>();
            //--------------------- product ---------------------------------------
            CreateMap<ProductAddDTO, Product>()
               
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product_Name))
            .ForMember(dest => dest.Descrption, opt => opt.MapFrom(src => src.Descrption))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))



            // Add any additional custom mappings here
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price));
            //--------------------- showProduct ------------------------------------
            CreateMap<ProductDTO, Product>();
            CreateMap<Product,ProductDTO>()
                 .ForMember(dest => dest.Product_Id, opt => opt.MapFrom(src => src.ProductId))
                  .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
                   .ForMember(dest => dest.Product_Name, opt => opt.MapFrom(src => src.Name))
                     .ForMember(dest => dest.Descrption, opt => opt.MapFrom(src => src.Descrption))
                      .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                    .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                    .ForMember(dest => dest.couponid, opt => opt.MapFrom(src => src.CoupnId))
                   
                     .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));
            //-------------------------- show producthascoupon---------------------------------------
            CreateMap<Product, prodcutHascouponDTO>()
                .ForMember(dest => dest.Product_Id, opt => opt.MapFrom(src => src.ProductId))
                 .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
                  .ForMember(dest => dest.Product_Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Descrption, opt => opt.MapFrom(src => src.Descrption))
                     .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                   .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                   .ForMember(dest => dest.couponid, opt => opt.MapFrom(src => src.CoupnId))
                   .ForMember(dest => dest.discount, opt => opt.MapFrom(src => src.Coupon.DiscountPercentage))

                    .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));
            //------------------------------------updateProduct----------------------------------
            CreateMap<UpdateProductDTO, Product>()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            //.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product_Id))
            //.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            //   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product_Name))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price));

            // ------------------- cartitem------------------
            CreateMap<CartItemDTO, CartItem>()

                .ForMember(dest => dest.ProId, opt => opt.MapFrom(src => src.ProductId));
            //    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quntity));

            //-------------------- showcartDEtitles---------------------------------
            CreateMap<Cart, CartDTO>()
        .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CartItems.Sum(x =>   x.TotalPrice)))
         .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.CartItems.Count()))
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems
      
        .Select(x => new CartItemDetilesDto
        {
            ProductId = x.Product.ProductId,
            ProductName = x.Product.Name,
            imageurl = x.Product.ImageURL,
            Quantity = x.Quantity,
            TotalPrice = x.TotalPrice,
            Stock=x.Product.Stock
           
        }).ToList()));

            //----------------- updatecartitem ------------------------------------------------
            CreateMap<updatecartitemDTO, CartItem>()
              .ForMember(dest => dest.ProId, opt => opt.MapFrom(src => src.productid))
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quntity));

            //---------------- create order--------------------------

            CreateMap<OrderDTO, Order>()
              .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
              .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            //------------------ create orderitem-------------------------------------
            CreateMap<OrderItemDTO, OrderItem>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
            //.ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId));
            //------------------ show the DEtitle order--------------------------------

            CreateMap<Order, OrderDetitlesDTO>()
           .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
             .ForMember(dest => dest.orderid, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.orderItems.Sum(x => x.Quantity * x.UnitPrice)))

          .ForMember(dest => dest.orderitemdatas, opt => opt.MapFrom(src => src.orderItems != null ? src.orderItems.Select(x => new orderitemdata
          {
              ProductId = x.product != null ? x.product.ProductId : 0,  // Provide a default value if product is null
              productname = x.product != null ? x.product.Name : string.Empty,
              Quantity = x.Quantity,
              TotalPrice = x.TotalPrice,
              UnitPrice = x.UnitPrice,
              //StripeCustomerId = x.Order.user != null && x.Order.user.StripeCustomerId != null ? x.Order.user.StripeCustomerId : string.Empty
          }).ToList() : new List<orderitemdata>()));

            // -----------------updateOrderitem------------------------------------------------
            CreateMap<updateOrderitemDTO, OrderItem>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            //------------------------- Category--------------------------------
            CreateMap<AddCategoryDTO, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Category, CategoryDTO>()
                 .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<CategoryDTO, Category>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryID))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            //----------------------- product Category----------------------------
            CreateMap<Product, productwithCategoryDTO>()
      .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.ProductId))
      .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.Name))
      .ForMember(dest => dest.categoryId, opt => opt.MapFrom(src => src.category != null ? src.category.Id : 0))
      .ForMember(dest => dest.categoryName, opt => opt.MapFrom(src => src.category != null ? src.category.Name : null));

            CreateMap<CartItem, OrderItem>()
         .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProId))
         .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
         .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price)) // Assuming you have Product details like price
         .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.Product.Price));


            //------------------------------------ copuns-------------------------------------------------------
            CreateMap<CouponDTO, Coupon>()
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
                                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate));
            CreateMap<updatecouponDTO, Coupon>()
               .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
                               .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                               .ForMember(dest => dest.CouponId, opt => opt.MapFrom(src => src.CouponId));
            CreateMap< Coupon,applaycopounDTO > ()
               .ForMember(dest => dest.CouponId, opt => opt.MapFrom(src => src.CouponId))
                               .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.products.Select(x => x.ProductId)));


            CreateMap<Coupon, showcoupondata>();

            //---------------------- payment order-------------------------------------
            CreateMap<OrderPaymentDTO, Order>()
       .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
      
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.orderid));

            CreateMap<AppUser, UserDataDTO>()
                .ForMember(dest => dest.userid, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
      
    }
}
