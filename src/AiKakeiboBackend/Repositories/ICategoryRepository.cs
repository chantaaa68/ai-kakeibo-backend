using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetCategoriesByKakeiboIdAsync(int kakeiboId);
        Task<List<CategoryDto>> GetDefaultCategoriesAsync();
        Task<Category?> GetByIdAsync(int categoryId);
        Task<Icon?> GetIconByNameAsync(string iconName);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
    }
}
