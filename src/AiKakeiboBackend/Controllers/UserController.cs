using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest req)
        {
            return await _service.LoginAsync(req);
        }

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserDataAsync([FromQuery] int UserId)
        {
            return await _service.GetUserDataAsync(UserId);
        }

        [HttpPost("Regist")]
        public async Task<IActionResult> RegistAsync([FromBody] RegistUserRequest req)
        {
            return await _service.RegistAsync(req);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest req)
        {
            return await _service.UpdateAsync(req);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteUserRequest req)
        {
            return await _service.DeleteAsync(req);
        }
    }
}
