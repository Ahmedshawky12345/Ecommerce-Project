using AutoMapper;
using Azure;
using Data.DTO;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineShopping.Repsitory.Interfaces;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepstory<Category> repstory;
        private readonly IMapper mapper;

        public CategoryController(IRepstory<Category> repstory,IMapper mapper)
        {
            this.repstory = repstory;
            this.mapper = mapper;
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoryDTO addCategoryDTO)
        {
            var response = new GenralResponse<string>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Failed to create Category .";
                response.Errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                return BadRequest(response);

            }
            var data = mapper.Map<Category>(addCategoryDTO);
            
            await repstory.AddAsync(data);
            response.Success = true;
            response.Message = "sucssfully Creat Category";
            return Ok(response);


        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var response = new GenralResponse<List<CategoryDTO>>();
            
           
          var category=  await repstory.GetAllAsync();
           // category == null ====> mean the data not return andy list , 
           // !category.any()=====> mean that the data not reutrn the data in list (null data in return list (list category inside metadata==null))
            if (category == null)
            {
                response.Success = false;
                response.Message = "no Category exist .";
                
                return BadRequest(response);
            }
            var data = mapper.Map<List<CategoryDTO>>(category);
            response.Success = true;
            response.Message = "sucssfully response";
            response.Data = data;
            return Ok(response);


        }
        [HttpGet("GetCategoryByID")]
        public async Task<IActionResult> GetCategoryById(int id )
        {
            var response = new GenralResponse<CategoryDTO>();


            var category = await repstory.GetByIdAsync(id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "this Category Not exis";

                return BadRequest(response);
            }
            var data = mapper.Map<CategoryDTO>(category);
            response.Success = true;
            response.Message = "sucssfully  response";
            response.Data = data;
            return Ok(response);


        }
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = new GenralResponse<string>();


            var category = await repstory.GetByIdAsync(id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "this Category Not exist .";

                return BadRequest(response);
            }
             repstory.Delete(category);
           
            response.Success = true;
            response.Message = " this category Removed sucssfully";
           
            return Ok(response);


        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO categoryDTO)
        {
            var response = new GenralResponse<string>();


            var category = await repstory.GetByIdAsync(categoryDTO.CategoryID);
            if (category == null)
            {
                response.Success = false;
                response.Message = "this Category Not exist .";

                return BadRequest(response);
            }
            mapper.Map(categoryDTO, category);
             repstory.Update(category);

            response.Success = true;
            response.Message = " this category update sucssfully";

            return Ok(response);


        }
    }
}
