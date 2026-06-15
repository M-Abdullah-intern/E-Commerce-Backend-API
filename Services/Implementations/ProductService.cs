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

        public async Task<PaginatedResult<ProductReadDto>> GetFilteredProducts(ProductQueryParams queryParams)
        {
            var result = await _repo.GetFilteredProductsAsync(queryParams);

            return new PaginatedResult<ProductReadDto>
            {
                Items = _mapper.Map<List<ProductReadDto>>(result.Items),
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
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

        public async Task UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Not found");

            product.UpdatedAt = DateTime.UtcNow;

            _mapper.Map(dto, product);

            var incomingUrls = dto.ProductImageCreateDtos
                .Select(i => i.ImageUrl)
                .ToList();

            var toRemove = product.ProductImages
                .Where(existing => !incomingUrls.Contains(existing.ImageUrl))
                .ToList();
            foreach (var img in toRemove)
            {
                product.ProductImages.Remove(img);
            }

            int? lastPrimaryIndex = null;

            foreach (var imageDto in dto.ProductImageCreateDtos)
            {
                var existing = product.ProductImages
                    .FirstOrDefault(e => e.ImageUrl == imageDto.ImageUrl);

                if (existing != null)
                {
                    existing.IsPrimary = imageDto.IsPrimary;
                }
                else
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = imageDto.ImageUrl,
                        IsPrimary = imageDto.IsPrimary,
                        ProductId = product.ProductId
                    });
                }

                if (imageDto.IsPrimary)
                {
                    lastPrimaryIndex = incomingUrls.IndexOf(imageDto.ImageUrl);
                }
            }

            if (lastPrimaryIndex.HasValue)
            {
                string winningUrl = incomingUrls[lastPrimaryIndex.Value];
                foreach (var img in product.ProductImages)
                {
                    img.IsPrimary = (img.ImageUrl == winningUrl);
                }
            }
            else
            {
                if (product.ProductImages.Any())
                {
                    var firstImage = product.ProductImages.First();
                    firstImage.IsPrimary = true;
                    foreach (var img in product.ProductImages.Skip(1))
                    {
                        img.IsPrimary = false;
                    }
                }
            }

            await _repo.UpdateAsync(product);
        }

        // Soft delete
        public async Task DeleteProduct(int id)
            => await _repo.DeleteAsync(id);
    }
}