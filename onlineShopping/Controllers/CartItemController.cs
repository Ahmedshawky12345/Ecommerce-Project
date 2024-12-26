using AutoMapper;
using Data.DTO.CartItem;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineShopping.Repsitory.Interfaces;
using System.Security.Claims;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CartItemController : ControllerBase
    {
        private readonly ICartItem repo;
        private readonly IMapper mapper;
        private readonly IRepstory<Product> repstory;
        private readonly ICart repocart;

        public CartItemController(ICartItem repo,IMapper mapper,IRepstory<Product> repstory,ICart repocart)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.repstory = repstory;
            this.repocart = repocart;
        }
        [HttpPost("CreateCartItem")]
        public async Task<IActionResult> AddCartItem(CartItemDTO cartItemDTO)
        {
            var response = new GenralResponse<string>();

            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to create cart item.";
                response.Errors = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                return BadRequest(response);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if a cart already exists for the user
            var existingCart = await repocart.GetCartByUserIdAsync(userId);
            if (existingCart == null)
            {
                // Create a new cart if none exists
                var cart = new Cart
                {
                    userid = userId
                };
                await repocart.CreateCartAsync(cart);
                existingCart = await repocart.GetCartByUserIdAsync(userId); // Fetch the newly created cart
            }

            // Check if the product exists
            var product = await repstory.GetByIdAsync(cartItemDTO.ProductId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found.";
                return BadRequest(response);
            }

            // Map DTO to CartItem
            var cartItem = mapper.Map<CartItem>(cartItemDTO);
            cartItem.TotalPrice = product.Price * cartItem.Quantity; // Calculate the total price

            try
            {
                // Add or update the cart item
                await repo.AddCartItemAsync(cartItem, existingCart.Id);
                response.Success = true;
                response.Message = "Successfully added or updated cart item.";
                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                // Handle exception
                response.Success = false;
                response.Message = "An error occurred while saving the cart item: " + ex.InnerException?.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpPut("updateCartItem")]
        public async Task<IActionResult> UpdateCartitem(updatecartitemDTO updatecartitemDTO)
        {
            var response = new GenralResponse<updatecartitemDTO>();

            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to create cart item.";
                response.Errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                return BadRequest(response);
            }
            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            // Retrieve the cart for the user
            var cart = await repocart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                response.Success = false;
                response.Message = "Cart not found for the logged-in user.";
                return NotFound(response);
            }
            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProId == updatecartitemDTO.productid);
            if (cartItem == null)
            {
                response.Success = false;
                response.Message = "Product not found in the cart.";
                return NotFound(response);
            }
            if (updatecartitemDTO.quntity > cartItem.Product.Stock)
            {
                response.Success = false;
                response.Message = "Stack of products not available";
               
                return NotFound(response);
            }

            var data = mapper.Map(updatecartitemDTO, cartItem);
            data.TotalPrice = data.Quantity * cartItem.Product.Price;
            await repo.updatecartitem(userId,data);

            response.Success = true;
            response.Message = "Product update from the cart successfully.";
            return Ok(response);
        }
        [HttpDelete("RemoveFromCartitem")]
        public async Task<IActionResult> RemoveFromCartitem(int productid)
        {
            var response = new GenralResponse<updatecartitemDTO>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }
            var product = await repo.GetCartItembyid(userId, productid);
            if (product == null)
            {
                response.Success = false;
                response.Message = "this product not exist in cart.";
                return NotFound(response);
            }
            try
            {
                await repo.RemoveFromCartitem(userId, productid);
                response.Success = true;
                response.Message = "Product removed from cart successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while removing the product from the cart.";
                response.Errors = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }

    }
}
