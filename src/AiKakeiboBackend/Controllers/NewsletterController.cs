using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterService _service;

        public NewsletterController(INewsletterService service)
        {
            _service = service;
        }

        [HttpPost("RegistNewsletter")]
        public async Task<IActionResult> RegistNewsletterAsync([FromBody] RegistNewsletterRequest req)
        {
            return await _service.RegistNewsletterAsync(req);
        }

        [HttpPost("UpdateNewsletter")]
        public async Task<IActionResult> UpdateNewsletterAsync([FromBody] UpdateNewsletterRequest req)
        {
            return await _service.UpdateNewsletterAsync(req);
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMailAsync([FromBody] SendNewsletterRequest req)
        {
            return await _service.SendMailAsync(req);
        }
    }
}
