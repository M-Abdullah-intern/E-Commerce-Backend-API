using ECommerceAPI.Data;
using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Helpers;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        // Database context for accessing the database
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get All Products method
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ToListAsync();
            return await _context.Products.
                Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        // Get Products by category method
        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
            => await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

        // Search Product by name method
        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
            => await _context.Products
                .Where(p => p.Name.Contains(keyword))
                .ToListAsync();

        // Get Productby ID method
        public async Task<Product> GetByIdAsync(int id)
        {            
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id && !p.IsDeleted);

            return product;
        }

        // Create Product method
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Update Product method
        public async Task UpdateAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }

        // Soft delete implementation
        public async Task DeleteAsync(int id)
        {
            // Find the product by ID
            var product = await _context.Products.FindAsync(id); 
            if (product == null) 
                return;

            // Soft delete: mark the product as deleted instead of removing it from the database
            product.IsDeleted = true; 
            product.UpdatedAt = DateTime.UtcNow; 
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Product>> GetFilteredProductsAsync(ProductQueryParams queryParams)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (queryParams.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);

            if (queryParams.MinPrice.HasValue)
                query = query.Where(p => p.Price >= queryParams.MinPrice.Value);

            if (queryParams.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);

            if (!string.IsNullOrEmpty(queryParams.Search))
                query = query.Where(p =>
                    p.Name.Contains(queryParams.Search) ||
                    p.Description.Contains(queryParams.Search));

            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                if (queryParams.SortBy.ToLower() == "price")
                {
                    query = queryParams.SortOrder == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                }
                else if (queryParams.SortBy.ToLower() == "name")
                {
                    query = queryParams.SortOrder == "desc"
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name);
                }
            }
            else
            {
                query = query.OrderBy(p => p.ProductId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(PaginationHelper.Skip(queryParams.PageNumber, queryParams.PageSize))
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PaginatedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.PageSize),
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }
    }
}