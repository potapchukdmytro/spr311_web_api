using Microsoft.AspNetCore.Mvc;
using spr311_web_api.BLL.Dtos.Product;
using spr311_web_api.BLL.Services.Product;
using spr311_web_api.DAL;

namespace spr311_web_api.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _productService.GetAllAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id);
            return product != null ? Ok(product) : BadRequest("Product not found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductDto dto)
        {
            var response = await _productService.CreateAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateProductDto dto)
        {
            var result = await _productService.UpdateAsync(dto);
            return result ? Ok("Product updated") : BadRequest("Product not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var result = await _productService.DeleteAsync(id);
            return result ? Ok("Product deleted") : BadRequest("Product not deleted");
        }
    }
}
