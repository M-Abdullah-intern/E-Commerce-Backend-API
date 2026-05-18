using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Helpers;
using ECommerceAPI.Models;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        // Fields
        private readonly IProductService _service;
        // Constructor
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // Public Methods
        // Get all products with filtering, sorting, and pagination
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _service.GetFilteredProducts(queryParams);
            if (products == null)
                return NotFound();
            return Ok(products);
        }

        // Get product by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }


        // Admin Methods
        // Admin create Product
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            var result = await _service.AddProduct(dto);
            return Ok(result);
        }

        // Admin update  product(full update)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto dto)
        {
            await _service.UpdateProduct(id, dto);
            return Ok(ApiResponseHelper.Success("Product updated successfully"));
        }

        // Soft delete product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteProduct(id);
            return Ok(ApiResponseHelper.Success(("Product deleted successfully")));
        }
    }
}