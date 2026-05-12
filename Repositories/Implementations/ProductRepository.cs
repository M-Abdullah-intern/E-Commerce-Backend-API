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
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
            => await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
            => await _context.Products
                .Where(p => p.Name.Contains(keyword))
                .ToListAsync();

        public async Task<Product> GetByIdAsync(int id)
            => await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductQueryParams queryParams)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // FILTER
            if (queryParams.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);

            if (queryParams.MinPrice.HasValue)
                query = query.Where(p => p.Price >= queryParams.MinPrice.Value);

            if (queryParams.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);

            // SORT
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
                query = query.OrderBy(p => p.Id); // default
            }

            // PAGINATION
            query = query
                .Skip(PaginationHelper.Skip(
                queryParams.PageNumber,
                queryParams.PageSize))
                .Take(queryParams.PageSize);

            return await query.ToListAsync();
        }
    }
}