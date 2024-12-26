using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineShopping.Repsitory.Interfaces;
using Data.DTO;
using AutoMapper;
using Data.DTO.CartItem;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IRepstory<Product> repstory;
        private readonly IMapper mapper;
        private readonly Iproduct productrepo;
        private readonly IRepstory<Category> catrepo;

        public ProductController(IWebHostEnvironment hostEnvironment,IRepstory<Product> repstory,IMapper mapper,Iproduct productrepo,IRepstory<Category>catrepo)
        {
            this.hostEnvironment = hostEnvironment;
            this.repstory = repstory;
            this.mapper = mapper;
            this.productrepo = productrepo;
            this.catrepo = catrepo;
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductAddDTO productAddDTO)
        {
            var response = new GenralResponse<string>();

            // Validate ModelState
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to add product";
                response.Errors = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                return BadRequest(response);
            }

            // Validate if image is provided
            if (productAddDTO.image == null || productAddDTO.image.Length == 0)
            {
                response.Success = false;
                response.Message = "Image is required";
                return BadRequest(response);
            }

            try
            {
                // Generate unique file name using GUID
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productAddDTO.image.FileName);
                var imagesDirectory = Path.Combine(hostEnvironment.WebRootPath, "Images");

                // Ensure the Images directory exists
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                // Define the full path to save the image
                var fullPath = Path.Combine(imagesDirectory, uniqueFileName);

                // Save the image to the server
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await productAddDTO.image.CopyToAsync(stream);
                }

                // Build the image URL
                var scheme = HttpContext.Request.Scheme;  // Get the protocol (http or https)
                var host = HttpContext.Request.Host;      // Get the host (localhost or domain)
                var imageUrl = $"{scheme}://{host}/Images/{uniqueFileName}";

                // Map DTO to Product entity
                var data = mapper.Map<Product>(productAddDTO);
                data.ImageURL = imageUrl;

                // Save the product data
                await repstory.AddAsync(data);

                response.Success = true;
                response.Message = "Successfully added product";
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file saving errors)
                response.Success = false;
                response.Message = "An error occurred while saving the product";
                response.Errors = new List<string> { ex.Message };
                return StatusCode(500, response);
            }
        }


        [HttpGet("Getproductbyid")]
        public async Task<IActionResult> GetPoductDetiles(int id)
        {
            var response = new GenralResponse<ProductDTO>();

            var products = await repstory.GetByIdAsync(id);
            if (products == null)
            {
                response.Success = false;
                response.Message = "no Product";
                return BadRequest(response);
            }
            var data = mapper.Map<ProductDTO>(products);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);

        }


        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var response = new GenralResponse<List<ProductDTO>>();

            var products = await repstory.GetAllAsync();
            if(products == null )
            {
                response.Success = false;
                response.Message = "no Product";
                return BadRequest(response);
            }
            var data= mapper.Map<List<ProductDTO>>(products);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data=data;
            return Ok(response);

        }

        [HttpGet("NewerProducts")]
        public async Task<IActionResult> Getnewerproducts()
        {
            var response = new GenralResponse<List<ProductDTO>>();

            var products = await productrepo.Getnewerproduct();
            if (products == null)
            {
                response.Success = false;
                response.Message = "no Product";
                return BadRequest(response);
            }
            var data = mapper.Map<List<ProductDTO>>(products);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);

        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            var response = new GenralResponse<string>();

            // Retrieve the product by ID
            var product = await repstory.GetByIdAsync(updateProductDTO.Product_Id.Value);
            if (product == null)
            {
                response.Success = false;
                response.Message = "No Product found";
                return BadRequest(response);
            }

            // Only update the image if a new image file is provided
            if (updateProductDTO.image != null && updateProductDTO.image.Length > 0)
            {
                var uniqueName = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(updateProductDTO.image.FileName);
                var uniqueFileName = $"{uniqueName}{fileExtension}";
                var fullPath = Path.Combine(hostEnvironment.WebRootPath, "Images", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await updateProductDTO.image.CopyToAsync(stream);
                }

                // Update the ImageURL with the new file path
                var scheme = HttpContext.Request.Scheme;
                var host = HttpContext.Request.Host;
                product.ImageURL = $"{scheme}://{host}/Images/{uniqueFileName}";
            }

            // Map other fields from DTO to product (excluding ImageURL if image is null)
            mapper.Map(updateProductDTO, product);

            // Update the product in the repository
            repstory.Update(product);

            response.Success = true;
            response.Message = "Successfully updated product";
            return Ok(response);
        }


        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = new GenralResponse<string>();

            var product = await repstory.GetByIdAsync(id);
            if (product == null)
            {
                response.Success = false;
                response.Message = "no Product";
                return BadRequest(response);
            }
            
            repstory.Delete(product);

            response.Success = true;
            response.Message = "sussfully Delete Product";

            return Ok(response);

        }

        [HttpGet("GetproductWithCategory")]
        public async Task<IActionResult> GetproductWithCategory(int id)
        {
            var response = new GenralResponse<List<ProductDTO>>();
            var category = await catrepo.GetByIdAsync(id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "id not valid";
                return BadRequest(response);

            }
            var products = await productrepo.GetproductbycategoryId(id);
            if (products == null)
            {
                response.Success = false;
                response.Message = "no Product";
                return BadRequest(response);
            }
            var data = mapper.Map<List<ProductDTO>>(products);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);

        }
        [HttpGet("ProductHasCoupon")]

        public async Task<IActionResult> GetproductHasCoupon() {
            var response = new GenralResponse<List<prodcutHascouponDTO>>();

            var products = await productrepo.GetproductHasCoupon();
            if (products == null || !products.Any())
            {
                response.Success = false;
                response.Message = "No products with coupons found.";
                return NotFound(response);
            }


            var data = mapper.Map<List<prodcutHascouponDTO>>(products);
            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);

        }
        [HttpGet("GetAllDataPagination")]
        public async Task<IActionResult> GetAllDataPagination([FromQuery] PaginationDTO paginationDTO)
        {
            var response = new GenralResponse<PagedResponse<ProductDTO>>();

            // Validate inputs (optional double-check)
            if (paginationDTO.PageNumber <= 0 || paginationDTO.PageSize <= 0)
            {
                response.Success = false;
                response.Message = "Invalid page number or page size.";
                return BadRequest(response);
            }

            // Get paginated products from repository
            var pagedProducts = await productrepo.GetAllproductwithPagination(paginationDTO.PageNumber, paginationDTO.PageSize);

            // Map the data to ProductDTO
            var pagedProductDTO = new PagedResponse<ProductDTO>
            {
                Items = mapper.Map<List<ProductDTO>>(pagedProducts.Items),
                TotalItems = pagedProducts.TotalItems,
                PageNumber = pagedProducts.PageNumber,
                PageSize = pagedProducts.PageSize,
                TotalPages = pagedProducts.TotalPages
            };

            response.Success = true;
            response.Message = "Successfully fetched paginated products.";
            response.Data = pagedProductDTO;

            return Ok(response);
        }

        [HttpGet("SearchProduct")]
       
        public async Task<IActionResult> SearchProduct([FromQuery] string quary)
        {
            var response = new GenralResponse<List<ProductDTO>>();
            if (string.IsNullOrEmpty(quary))
            {
                response.Success = false;
                response.Message = "Search query cannot be empty";
                return BadRequest(response);
            }
                var products = await productrepo.SearchProducts(quary);
            if (products == null || !products.Any())
            {
                response.Success = false;
                response.Message = "No products  found.";
                return NotFound(response);
            }


            var data = mapper.Map<List<ProductDTO>>(products);

            response.Success = true;
            response.Message = "sussfully response";
            response.Data = data;
            return Ok(response);
        }

    }
}
