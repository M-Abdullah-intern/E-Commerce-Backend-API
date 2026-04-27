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
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _service.GetFilteredProducts(queryParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }


        // Admin Methods
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            await _service.AddProduct(dto);
            return Ok(ApiResponseHelper.Success("Product added successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto dto)
        {
            await _service.UpdateProduct(id, dto);
            return Ok(ApiResponseHelper.Success("Product updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteProduct(id);
            return Ok(ApiResponseHelper.Success(("Product deleted successfully")));
        }
    }
}