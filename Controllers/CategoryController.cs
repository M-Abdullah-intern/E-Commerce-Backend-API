using ECommerceAPI.DTOs.Category;
using ECommerceAPI.Helpers;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/category")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET ALL CATEGORIES
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAll();
            return Ok(categories);
        }

        // CREATE CATEGORY
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            await _service.CreateCategory(category);

            return Ok(ApiResponseHelper.Success("Category created successfully"));
        }
    }
}