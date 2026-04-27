using AutoMapper;
using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        
        // User Methods
        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var products = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<IEnumerable<ProductReadDto>> GetProductsByCategory(int categoryId)
        {
            var products = await _repo.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<IEnumerable<ProductReadDto>> SearchProducts(string keyword)
        {
            var products = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetProductById(int id)
        {
            var products = await _repo.GetByIdAsync(id);
            return _mapper.Map<ProductReadDto>(products);
        }

        public async Task<IEnumerable<ProductReadDto>> GetFilteredProducts(ProductQueryParams queryParams)
        {
            var products = await _repo.GetFilteredProductsAsync(queryParams);
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        // Admin Methods
        public async Task AddProduct(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _repo.AddAsync(product);
        }

        public async Task UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Not found");

            _mapper.Map(dto, product);

            await _repo.UpdateAsync(product);
        }

        public async Task DeleteProduct(int id)
            => await _repo.DeleteAsync(id);
    }
}