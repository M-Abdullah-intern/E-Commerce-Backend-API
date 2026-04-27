using AutoMapper;
using ECommerceAPI.DTOs.Category;
using ECommerceAPI.Exceptions;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;

namespace ECommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task CreateCategory(Category category)
        {
            // CHECK BEFORE INSERT
            var exists = await _categoryRepository.Exists(category.Name.Trim().ToLower());

            if (exists)
            {
                throw new BadRequestException("Category already exists");
            }

            await _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}