using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    public interface IUserRepository
    {
        Task<Users?> GetByEmailAndHashAsync(string email, string userHash);
        Task<Users?> GetByIdAsync(int userId);
        Task<Users?> GetByEmailAsync(string email);
        Task<Users> CreateUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int userId);
        Task<Kakeibo?> GetKakeiboByUserIdAsync(int userId);
        Task<Kakeibo> CreateKakeiboAsync(Kakeibo kakeibo);
        Task UpdateKakeiboAsync(Kakeibo kakeibo);
    }
}
