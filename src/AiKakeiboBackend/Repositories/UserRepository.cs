using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KakeiboDbContext _context;

        public UserRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetByEmailAndHashAsync(string email, string userHash)
        {
            return await _context.Users
                .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                .FirstOrDefaultAsync(u => u.Email == email
                                          && u.UserHash == userHash
                                          && u.DeleteDate == null);
        }

        public async Task<Users?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeleteDate == null);
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.DeleteDate == null);
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            user.CreateDate = DateTime.UtcNow;
            user.UpdateDate = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(Users user)
        {
            user.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.DeleteDate = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Kakeibo?> GetKakeiboByUserIdAsync(int userId)
        {
            return await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.UserId == userId && k.DeleteDate == null);
        }

        public async Task<Kakeibo> CreateKakeiboAsync(Kakeibo kakeibo)
        {
            kakeibo.CreateDate = DateTime.UtcNow;
            kakeibo.UpdateDate = DateTime.UtcNow;
            _context.Kakeibos.Add(kakeibo);
            await _context.SaveChangesAsync();
            return kakeibo;
        }

        public async Task UpdateKakeiboAsync(Kakeibo kakeibo)
        {
            kakeibo.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
