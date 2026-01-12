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

        /// <summary>
        /// ニュースレター登録APIです。新規ニュースレター情報を登録します。
        /// ユーザーへのメール配信用のニュースレター内容を作成する際に使用します。
        /// </summary>
        /// <param name="req">ニュースレター登録リクエスト（Title, Content等）</param>
        /// <returns>登録結果（登録されたニュースレター情報）</returns>
        [HttpPost("RegistNewsletter")]
        public async Task<IActionResult> RegistNewsletterAsync([FromBody] RigistNewsletterRequest req)
        {
            return await _service.RegistNewsletterAsync(req);
        }

        /// <summary>
        /// ニュースレター更新APIです。指定されたニュースレターIDのニュースレター情報（タイトル、内容等）を更新します。
        /// </summary>
        /// <param name="req">ニュースレター更新リクエスト（NewsletterId, Title, Content等）</param>
        /// <returns>更新結果</returns>
        [HttpPost("UpdateNewsletter")]
        public async Task<IActionResult> UpdateNewsletterAsync([FromBody] UpdateNewsletterRequest req)
        {
            return await _service.UpdateNewsletterAsync(req);
        }

        /// <summary>
        /// ニュースレターメール送信APIです。指定されたニュースレターを登録ユーザーに一斉送信します。
        /// </summary>
        /// <param name="req">ニュースレター送信リクエスト（NewsletterId等）</param>
        /// <returns>送信結果</returns>
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMailAsync([FromBody] SendNewsletterRequest req)
        {
            return await _service.SendMailAsync(req);
        }
    }
}
