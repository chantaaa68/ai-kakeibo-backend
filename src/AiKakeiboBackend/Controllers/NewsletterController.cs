using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly KakeiboDbContext _context;

        public NewsletterController(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ニュースレター登録
        /// </summary>
        [HttpPost("RigistNewsletter")]
        public async Task<ActionResult<ApiResponse<object>>> RigistNewsletter([FromBody] RegistNewsletterRequest request)
        {
            try
            {
                var newsletter = new NewsletterTemplate
                {
                    MailTitle = request.Title ?? string.Empty,
                    MailBody = request.MailBody ?? string.Empty,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.NewsletterTemplates.Add(newsletter);
                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "ニュースレターを登録しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"ニュースレター登録中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ニュースレター更新
        /// </summary>
        [HttpPost("UpdateNewsletter")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateNewsletter([FromBody] UpdateNewsletterRequest request)
        {
            try
            {
                var newsletter = await _context.NewsletterTemplates
                    .FirstOrDefaultAsync(n => n.Id == request.Id && n.DeleteDate == null);

                if (newsletter == null)
                {
                    return Ok(HttpResponseService.Fail<object>("ニュースレターが見つかりません"));
                }

                newsletter.MailTitle = request.Title ?? string.Empty;
                newsletter.MailBody = request.MailBody ?? string.Empty;
                newsletter.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "ニュースレターを更新しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"ニュースレター更新中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ニュースレター送信
        /// </summary>
        [HttpPost("SendMail")]
        public async Task<ActionResult<ApiResponse<object>>> SendMail([FromBody] SendNewsletterRequest request)
        {
            try
            {
                // ニュースレターテンプレート取得
                var newsletter = await _context.NewsletterTemplates
                    .FirstOrDefaultAsync(n => n.Id == request.NewsletterId && n.DeleteDate == null);

                if (newsletter == null)
                {
                    return Ok(HttpResponseService.Fail<object>("ニュースレターが見つかりません"));
                }

                // ユーザー情報取得
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeleteDate == null);

                if (user == null)
                {
                    return Ok(HttpResponseService.Fail<object>("ユーザーが見つかりません"));
                }

                // アイテム情報取得
                var item = await _context.KakeiboItems
                    .Include(i => i.Category)
                    .FirstOrDefaultAsync(i => i.Id == request.ItemId && i.DeleteDate == null);

                if (item == null)
                {
                    return Ok(HttpResponseService.Fail<object>("アイテムが見つかりません"));
                }

                // メール送信処理
                // 注意: 実際のメール送信処理はここで実装する必要があります
                // 現時点ではログ出力のみ
                var mailContent = newsletter.MailBody
                    .Replace("{UserName}", user.Name)
                    .Replace("{ItemName}", item.ItemName ?? string.Empty)
                    .Replace("{ItemAmount}", item.ItemAmount.ToString());

                // TODO: 実際のメール送信処理を実装
                // 例: await _emailService.SendAsync(user.Email, newsletter.MailTitle, mailContent);

                return Ok(HttpResponseService.Ok<object>(null, "ニュースレターを送信しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"ニュースレター送信中にエラーが発生しました: {ex.Message}"));
            }
        }
    }
}
