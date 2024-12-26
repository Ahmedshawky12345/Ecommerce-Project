using AutoMapper;
using Data.DTO;
using Data.DTO.CartItem;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineShopping.Repsitory.Interfaces;
using System.Security.Claims;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICart repo;
        private readonly IMapper mapper;

        public CartController(ICart repo,IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        //[HttpPost("CreateCart")]
        //public async Task<IActionResult> CreateCart()
        //{
        //    var response = new GenralResponse<CartItemDTO>();
        //    if (!ModelState.IsValid)
        //    {
        //        response.Success = false;
        //        response.Message = "falid to Create Cart";
        //        response.Errors = ModelState.Values
        //                                    .SelectMany(v => v.Errors)
        //                                    .Select(e => e.ErrorMessage)
        //                                    .ToList();
        //        return BadRequest(response);

        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var cart = new Cart
        //    {
        //        userid = userId
        //    };
            
        //    await repo.CreateCartAsync(cart);

        //    response.Success = true;
        //    response.Message = "sucssfully Creat cart";
        //    return Ok(response);


        //}
        [HttpGet]
        public async Task<IActionResult> GetCartByUserId()
        {
            var response = new GenralResponse<CartDTO>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "No Data";
                response.Errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                return BadRequest(response);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }
            var existcart = await repo.GetCartByUserIdAsync(userId);
            if (existcart == null)
            {
                response.Success = false;
                response.Message = "This user does not have a cart.";
                return BadRequest(response);
            }

            var cartDto = mapper.Map<CartDTO>(existcart);
            
            response.Success = true;
            response.Data = cartDto;
            response.Message = "respose sussfully";
            return Ok(response);
        }

        [HttpDelete("cart/product/{productId}")]
        public async Task<IActionResult> DeleteItemFromCart(int productId)
        {
            var response = new GenralResponse<string>();

            // Retrieve the user's ID from the authentication context
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not logged in.";
                return Unauthorized(response);
            }

            // Retrieve the cart for the user
            var cart = await repo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                response.Success = false;
                response.Message = "Cart not found for the logged-in user.";
                return NotFound(response);
            }

            // Find the product in the cart
            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProId == productId);
            if (cartItem == null)
            {
                response.Success = false;
                response.Message = "Product not found in the cart.";
                return NotFound(response);
            }

            // Remove the item from the cart
            await repo.DeleteCartItem(userId, productId);
                        
            response.Success = true;
            response.Message = "Product removed from the cart successfully.";
            return Ok(response);
        }



    }
}

