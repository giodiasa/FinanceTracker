using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryRequestDto model)
        {
            int userId = GetUserId();
            await _categoryService.AddCategoryAsync(userId, model);
            return Created();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            int userId = GetUserId();
            var categories = await _categoryService.GetAllCategoriesByUserId(userId);
            return Ok(categories);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequestDto model)
        {
            int userId = GetUserId();
            await _categoryService.UpdateCategoryAsync(id, userId, model);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            int userId = GetUserId();
            await _categoryService.DeleteCategoryAsync(id, userId);
            return NoContent();
        }
    }
}
