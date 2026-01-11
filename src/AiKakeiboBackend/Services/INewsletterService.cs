using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface INewsletterService
    {
        Task<IActionResult> RegistNewsletterAsync(RegistNewsletterRequest request);
        Task<IActionResult> UpdateNewsletterAsync(UpdateNewsletterRequest request);
        Task<IActionResult> SendMailAsync(SendNewsletterRequest request);
    }
}
