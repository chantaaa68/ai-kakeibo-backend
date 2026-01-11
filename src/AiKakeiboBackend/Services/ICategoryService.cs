using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface ICategoryService
    {
        Task<IActionResult> GetCategoryDataAsync(GetCategoryDataRequest request);
        Task<IActionResult> RegistCategoryAsync(RegistCategoryRequest request);
        Task<IActionResult> UpdateCategoryAsync(UpdateCategoryRequest request);
    }
}
