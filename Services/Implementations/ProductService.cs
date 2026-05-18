using AutoMapper;
using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        // Dependencies
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        
        // User Methods
        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Get all with related data
        public async Task<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            // Fetch all products with related data (e.g., category, images)
            var products = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }
        
        // Get by category
        public async Task<IEnumerable<ProductReadDto>> GetProductsByCategory(int categoryId)
        {
            var products = await _repo.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        // Search with relevance ranking (basic implementation)
        public async Task<IEnumerable<ProductReadDto>> SearchProducts(string keyword)
        {
            var products = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        // Get by ID with related data
        public async Task<ProductReadDto> GetProductById(int id)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("Invalid product ID");

            // Fetch product with related data (e.g., category, images)
            var products = await _repo.GetByIdAsync(id);
            return _mapper.Map<ProductReadDto>(products);
        }

        // Advanced filtering with pagination
        public async Task<IEnumerable<ProductReadDto>> GetFilteredProducts(ProductQueryParams queryParams)
        {
            // Validate query parameters (e.g., page number, page size)
            var products = await _repo.GetFilteredProductsAsync(queryParams);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        // Admin Methods
        // Create
        public async Task<ProductCreatedDto> AddProduct(ProductCreateDto dto)
        {
            // Map DTO to entity
            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;
            await _repo.AddAsync(product);

            return new ProductCreatedDto
            {
                ProductId = product.ProductId,
                Message = "Product added successfully"
            };
        }

        // Update with optimistic concurrency
        public async Task UpdateProduct(int id, ProductUpdateDto dto)
        {
            // Fetch existing product
            var product = await _repo.GetByIdAsync(id);

            product.UpdatedAt = DateTime.UtcNow;

            // Check if product exists
            if (product == null)
                throw new Exception("Not found");

            _mapper.Map(dto, product);

            await _repo.UpdateAsync(product);
        }

        // Soft delete
        public async Task DeleteProduct(int id)
            => await _repo.DeleteAsync(id);
    }
}