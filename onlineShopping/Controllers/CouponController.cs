using AutoMapper;
using Data.DTO.Coupons;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using onlineShopping.Repsitory.Interfaces;


namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IRepstory<Coupon> repo;
        private readonly IMapper mapper;
        private readonly ICoupon repocoupon;
        private readonly IRepstory<Product> repoproduct;

        public CouponController(IRepstory<Coupon> repo,IMapper mapper,ICoupon repocoupon,IRepstory<Product> repoproduct )
        {
            this.repo = repo;
            this.mapper = mapper;
            this.repocoupon = repocoupon;
            this.repoproduct = repoproduct;
        }

        private string GenerateUniqueCode()
        {
            return $"COUPON-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddCoupon(CouponDTO couondto)
        {
            var response = new GenralResponse<string>();

            // Validate ModelState
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to add coupon";
                response.Errors = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                return BadRequest(response);
            }
           
            var couponmaped = mapper.Map<Coupon>(couondto);
           
            couponmaped.Code = GenerateUniqueCode();
           await repo.AddAsync(couponmaped);

            response.Success = true;
            response.Message = "coupon genrate succesfully";
            return BadRequest(response);




        }





        [HttpGet("GetAllCoupon")]
        public async Task<IActionResult> GetAllcoupon()
        {
            var response = new GenralResponse<List<showcoupondata>>();

            var coupons = await repo.GetAllAsync();
            if (coupons == null)
            {
                response.Success = false;
                response.Message = "no coupon";
                return BadRequest(response);
            }
            var data = mapper.Map<List<showcoupondata>>(coupons);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);

        }


        [HttpPut("Updatecoupon")]
        public async Task<IActionResult> Updatecoupon( updatecouponDTO updatecouponDTO)
        {
            var response = new GenralResponse<string>();

            // Retrieve the product by ID
            var coupon = await repo.GetByIdAsync(updatecouponDTO.CouponId);
            if (coupon == null)
            {
                response.Success = false;
                response.Message = "No coupon found";
                return BadRequest(response);
            }

            

           
            mapper.Map(updatecouponDTO, coupon);

            repo.Update(coupon);

            response.Success = true;
            response.Message = "Successfully updated coupon";
            return Ok(response);
        }


        [HttpDelete("Deletecoupon")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = new GenralResponse<string>();
            
            var coupon = await repo.GetByIdAsync(id);
            if (coupon == null)
            {
                response.Success = false;
                response.Message = "no coupon";
                return BadRequest(response);
            }

            repo.Delete(coupon);

            response.Success = true;
            response.Message = "sussfully Delete coupon";

            return Ok(response);

        }

        [HttpPost("ApplayCouponToProduct")]
        public async Task<IActionResult> ApplayCouponToProduct(applaycopounDTO applaycopounDTO)
        {
            var response = new GenralResponse<string>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to add coupon";
                response.Errors = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                return BadRequest(response);
            }
            // cheack if coupon found or not
            var coupon = await repo.GetByIdAsync(applaycopounDTO.CouponId);
            if (coupon == null)
            {
                response.Success = false;
                response.Message = "no coupon ";
                return NotFound(response);
            }

            foreach(var i in applaycopounDTO.ProductIds)
            {
                var product = await repoproduct.GetByIdAsync(i);
                if (product == null )
                {
                    response.Success = false;
                    response.Message = $"Product {i} dont exist in products.";
                    return BadRequest(response);
                }
                if (product.CoupnId != null)
                {
                    response.Success = false;
                    response.Message = $"Product {product.ProductId} already has a coupon applied.";
                    return BadRequest(response);
                }
                product.CoupnId = applaycopounDTO.CouponId; // Apply the coupon to the product
                repoproduct.Update(product);
            }
       
            response.Success = true;
            response.Message = "Coupon successfully applied to products.";
            return Ok(response);



        }
        [HttpPost("RemoveCouponFromProduct")]
        public async Task<IActionResult> RemoveCouponFromProduct(int productId)
        {
            var response = new GenralResponse<string>();

            // Check if the product exists
            var product = await repoproduct.GetByIdAsync(productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = $"Product {productId} does not exist.";
                return BadRequest(response);
            }

            // Check if the product has a coupon applied
            if (product.CoupnId == null)
            {
                response.Success = false;
                response.Message = $"Product {productId} does not have a coupon applied.";
                return BadRequest(response);
            }

            // Remove the coupon from the product
            await repocoupon.RemoveCouponFromProduct(productId);

            response.Success = true;
            response.Message = "Coupon successfully removed from the product.";
            return Ok(response);
        }

    }
}
