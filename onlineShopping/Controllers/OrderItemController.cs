using AutoMapper;
using Data.DTO.CartItem;
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
        private readonly IMapper mapper;
        private readonly Iorder repstoryorder;
        private readonly IRepstory<Product> repstoryproduct;
        private readonly IOrderItem RepoOrderitem;
        //private readonly IPaymobRepository paymobRepository;

        public OrderItemController(ICart repocart
            , IMapper mapper, Iorder repstoryorder, IRepstory<Product> repstoryproduct, IOrderItem RepoOrderitem)
        {
            this.repstory = repstory;
            this.repocart = repocart;
            this.mapper = mapper;
            this.repstoryorder = repstoryorder;
            this.repstoryproduct = repstoryproduct;
            this.RepoOrderitem = RepoOrderitem;
          
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            var response = new GenralResponse<string>();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            // Retrieve the cart by userId
            var cart = await repocart.GetCartByUserIdAsync(userId);
            if (cart == null || cart.CartItems == null || cart.CartItems.Count == 0)
            {
                response.Success = false;
                response.Message = "No items in the cart to convert to order.";
                return BadRequest(response);
            }

            // Check if an existing order exists for this user
            var existingOrder = await repstoryorder.GetOrdersByUserIdAsync(userId);
            if (existingOrder == null)
            {
                var newOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 0 // Start with 0, total will be updated after processing items
                };
                await repstoryorder.AddAsync(newOrder);
                existingOrder = await repstoryorder.GetOrdersByUserIdAsync(userId);
            }

            decimal totalAmount = 0;

            // Process each item in the cart
            foreach (var cartItem in cart.CartItems)
            {
                // Check if this product already exists in the current order
                var existingOrderItem = await RepoOrderitem.GetByOrderIdAndProductIdAsync(existingOrder.Id, cartItem.Product.ProductId);

                if (existingOrderItem != null)
                {
                    // Update quantity and total price
                    existingOrderItem.Quantity += cartItem.Quantity;
                    existingOrderItem.TotalPrice = existingOrderItem.Quantity * existingOrderItem.UnitPrice;
                    await RepoOrderitem.UpdateAsync(userId, existingOrderItem);
                }
                else
                {
                    // Create a new OrderItem
                    var newOrderItem = mapper.Map<OrderItem>(cartItem);
                    newOrderItem.OrderId = existingOrder.Id;
                    newOrderItem.UnitPrice = cartItem.Product.Price; // Apply any discounts here
                    newOrderItem.TotalPrice = newOrderItem.UnitPrice * newOrderItem.Quantity;

                    await RepoOrderitem.AddAsync(newOrderItem);
                }

                // Update the total amount for the order
                totalAmount += cartItem.Quantity * cartItem.Product.Price; // Adjust for discounts if needed
            }

            // Update the TotalAmount of the order
            existingOrder.TotalAmount = totalAmount;
            await repstoryorder.UpdateAsync(existingOrder);

            response.Success = true;
            response.Message = "Order created/updated successfully from cart items.";
            return Ok(response);
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
