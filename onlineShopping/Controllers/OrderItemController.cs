using AutoMapper;
using Data.DTO.CartItem;
using Data.DTO.Coupons;
using Data.DTO.OrderItem;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineShopping.Repsitory;
using onlineShopping.Repsitory.Interfaces;
using System.Security.Claims;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IRepstory<OrderItem> repstory;
        private readonly ICart repocart;
        private readonly Iproduct productrepo;
        private readonly IMapper mapper;
        private readonly Iorder repstoryorder;
        private readonly IRepstory<Product> repstoryproduct;
        private readonly IOrderItem RepoOrderitem;
        private readonly ICoupon repocoupon;

        //private readonly IPaymobRepository paymobRepository;

        public OrderItemController(ICart repocart,Iproduct productrepo
            , IMapper mapper, Iorder repstoryorder, IRepstory<Product> repstoryproduct, IOrderItem RepoOrderitem,ICoupon repocoupon)
        {
            this.repstory = repstory;
            this.repocart = repocart;
            this.productrepo = productrepo;
            this.mapper = mapper;
            this.repstoryorder = repstoryorder;
            this.repstoryproduct = repstoryproduct;
            this.RepoOrderitem = RepoOrderitem;
            this.repocoupon = repocoupon;
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            var response = new GenralResponse<string>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            try
            {
                // Retrieve the cart for the user
                var cart = await repocart.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    response.Success = false;
                    response.Message = "Cart is empty. Cannot create order.";
                    return BadRequest(response);
                }

                // Create a new order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 0
                };
                await repstoryorder.AddAsync(order);

                decimal totalAmount = 0;

                // Retrieve products and their coupons in one query
                var productIds = cart.CartItems.Select(ci => ci.Product.ProductId).ToList();
                var products = await productrepo.GetProductsWithCouponsAsync(productIds);

                foreach (var cartItem in cart.CartItems)
                {
                    var product = products.FirstOrDefault(p => p.ProductId == cartItem.Product.ProductId);
                    if (product == null) continue;

                    // Apply discount if coupon is valid
                    decimal unitPrice = product.Price;
                    if (product.Coupon != null && product.Coupon.ExpiryDate >= DateTime.UtcNow)
                    {
                        unitPrice -= product.Coupon.DiscountPercentage;
                        unitPrice = Math.Max(unitPrice, 0); // Ensure non-negative price
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = product.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = unitPrice * cartItem.Quantity
                    };

                    await RepoOrderitem.AddAsync(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }

                // Update the order total
                order.TotalAmount = totalAmount;
                await repstoryorder.UpdateAsync(order);

                // Clear the cart after order is created
                await repocart.ClearCartAsync(userId);

                response.Success = true;
                response.Message = "Order created successfully and cart cleared.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        [HttpPut("UpdateOrderItem")]
        public async Task<IActionResult> UpdataeOrderitem(updateOrderitemDTO updateOrderitemDTO)
        {
            var response = new GenralResponse<updatecartitemDTO>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "falid to update OrderItem";
                response.Errors = ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .ToList();
                return BadRequest(response);

            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            // Retrieve the cart for the user
            var order = await repstoryorder.GetOrdersByUserIdAsync(userId);
            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found for the logged-in user.";
                return NotFound(response);
            }
            var orderitem = order.orderItems?.FirstOrDefault(ci => ci.ProductId == updateOrderitemDTO.ProductId);
            if (orderitem == null)
            {
                response.Success = false;
                response.Message = "Product not found in the cart.";
                return NotFound(response);
            }

            var data = mapper.Map(updateOrderitemDTO, orderitem);
            data.TotalPrice = data.Quantity * data.UnitPrice;
            await RepoOrderitem.UpdateAsync(userId, data);

            response.Success = true;
            response.Message = "Product update from the cart successfully.";
            return Ok(response);
        }
    }
}
