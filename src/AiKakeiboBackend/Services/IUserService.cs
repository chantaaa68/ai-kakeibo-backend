using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface IUserService
    {
        Task<IActionResult> LoginAsync(LoginRequest request);
        Task<IActionResult> GetUserDataAsync(int userId);
        Task<IActionResult> RegistAsync(RegistUserRequest request);
        Task<IActionResult> UpdateAsync(UpdateUserRequest request);
        Task<IActionResult> DeleteAsync(DeleteUserRequest request);
    }
}
