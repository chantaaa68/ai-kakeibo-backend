using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KakeiboDbContext _context;

        public CategoryRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetCategoriesByKakeiboIdAsync(int kakeiboId)
        {
            return await _context.Categories
                .Include(c => c.Icon)
                .Where(c => c.KakeiboID == kakeiboId && c.DeleteDate == null)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    InoutFlg = c.InoutFlg,
                    IconName = c.Icon.DefaultIconName,
                    IconId = c.IconId
                })
                .ToListAsync();
        }

        public async Task<List<CategoryDto>> GetDefaultCategoriesAsync()
        {
            return await _context.CategoryDefaults
                .Include(c => c.Icon)
                .Where(c => c.DeleteDate == null)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    CategoryName = c.KategoryName,
                    InoutFlg = c.InoutFlg,
                    IconName = c.Icon.DefaultIconName,
                    IconId = c.IconId
                })
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.DeleteDate == null);
        }

        public async Task<Icon?> GetIconByNameAsync(string iconName)
        {
            return await _context.Icons
                .FirstOrDefaultAsync(i => i.DefaultIconName == iconName && i.DeleteDate == null);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            category.CreateDate = DateTime.UtcNow;
            category.UpdateDate = DateTime.UtcNow;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            category.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
