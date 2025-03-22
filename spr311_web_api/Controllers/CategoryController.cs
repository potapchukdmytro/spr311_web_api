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
            bool result = await _categoryService.CreateAsync(dto);
            return result ? Ok($"Категорія '{dto.Name}' створена") : BadRequest("Помилка під час створення");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryDto dto)
        {
            bool result = await _categoryService.UpdateAsync(dto);
            return result ? Ok($"Категорія '{dto.Name}' оновлена") : BadRequest("Помилка під час оновлення");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("Id не отримано");
            }

            bool result = await _categoryService.DeleteAsync(id);
            return result ? Ok($"Категорія видалена") : BadRequest("Помилка під час видалення");
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id, string? name)
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name))
            {
                var result = await _categoryService.GetAllAsync();
                return Ok(result);
            }

            CategoryDto? dto = null;

            if(!string.IsNullOrEmpty(id))
            {
                dto = await _categoryService.GetByIdAsync(id);
            }
            
            if(dto == null && !string.IsNullOrEmpty(name))
            {
                dto = await _categoryService.GetByNameAsync(name);
            }

            return dto != null ? Ok(dto) : BadRequest("Категорію не знайдено");
        }

    }
}
