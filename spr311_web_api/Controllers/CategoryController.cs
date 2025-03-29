using Microsoft.AspNetCore.Mvc;
using spr311_web_api.BLL.Dtos.Category;
using spr311_web_api.BLL.Services.Category;

namespace spr311_web_api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCategoryDto dto)
        {
            var response = await _categoryService.CreateAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryDto dto)
        {
            var response = await _categoryService.UpdateAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("Id не отримано");
            }

            var response = await _categoryService.DeleteAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id, string? name)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var responseId = await _categoryService.GetByIdAsync(id);
                return responseId.IsSuccess ? Ok(responseId) : BadRequest(responseId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                var responseName = await _categoryService.GetByNameAsync(name);
                return responseName.IsSuccess ? Ok(responseName) : BadRequest(responseName);
            }


            var response = await _categoryService.GetAllAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

    }
}
