using AutoMapper;
using Data.DTO.CartItem;
using Data.DTO.Order;

using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using onlineShopping.Repsitory;
using onlineShopping.Repsitory.Interfaces;
using System.Security.Claims;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly Iorder repository;
        

        public OrderController(IMapper mapper, Iorder repository)
        {
            this.mapper = mapper;
            this.repository = repository;
    
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            var response = new GenralResponse<string>();

            if (!ModelState.IsValid)
            {
                return BadRequest(GetValidationErrors());
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                response.Success = false;
                response.Message = "Cart not found for the logged-in user.";
                return NotFound(response);
            }

            var order = mapper.Map<Order>(orderDTO);
            order.UserId = userId;
            await repository.AddAsync(order);

            response.Success = true;
            response.Message = "Successfully created order.";
            return Ok(response);
        }

        private GenralResponse<string> GetValidationErrors()
        {
            var response = new GenralResponse<string>
            {
                Success = false,
                Message = "Failed to create order.",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            };
            return response;
        }

        [HttpDelete("Order/product/{productId}")]
        public async Task<IActionResult> DeleteItemFromOrder(int productId)
        {
            var response = new GenralResponse<string>();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            var order = await repository.GetOrdersByUserIdAsync(userId);
            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found for the logged-in user.";
                return NotFound(response);
            }

            var orderItem = order.orderItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (orderItem == null)
            {
                response.Success = false;
                response.Message = "Product not found in the order.";
                return NotFound(response);
            }

            await repository.DeleteOrderItem(userId, productId);

            response.Success = true;
            response.Message = "Product removed from the order successfully.";
            return Ok(response);
        }

        [HttpGet("GetOrderLoginUser")]
        public async Task<IActionResult> GetOrderByUserId()
        {
            var response = new GenralResponse<OrderDetitlesDTO>();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var order = await repository.GetAllOrderDetiles(userId);
            if (order == null)
            {
                response.Success = false;
                response.Message = "This user does not have any orders.";
                return NotFound(response);
            }

            var orderDto = mapper.Map<OrderDetitlesDTO>(order);
            response.Success = true;
            response.Data = orderDto;
            response.Message = "Response successfully.";
            return Ok(response);
        }

       
    }

}
