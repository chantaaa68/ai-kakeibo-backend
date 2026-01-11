using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public class NewsletterService : INewsletterService
    {
        private readonly INewsletterRepository _newsletterRepository;

        public NewsletterService(INewsletterRepository newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        public async Task<IActionResult> RegistNewsletterAsync(RegistNewsletterRequest request)
        {
            try
            {
                var newsletter = new NewsletterTemplate
                {
                    MailTitle = request.Title ?? string.Empty,
                    MailBody = request.MailBody ?? string.Empty
                };

                await _newsletterRepository.CreateNewsletterAsync(newsletter);

                return ApiResponseHelper.Success<object>(null, "ニュースレターを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター登録中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateNewsletterAsync(UpdateNewsletterRequest request)
        {
            try
            {
                var newsletter = await _newsletterRepository.GetByIdAsync(request.Id);

                if (newsletter == null)
                {
                    return ApiResponseHelper.Fail("ニュースレターが見つかりません");
                }

                newsletter.MailTitle = request.Title ?? string.Empty;
                newsletter.MailBody = request.MailBody ?? string.Empty;

                await _newsletterRepository.UpdateNewsletterAsync(newsletter);

                return ApiResponseHelper.Success<object>(null, "ニュースレターを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター更新中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> SendMailAsync(SendNewsletterRequest request)
        {
            try
            {
                // ニュースレターテンプレート取得
                var newsletter = await _newsletterRepository.GetByIdAsync(request.NewsletterId);

                if (newsletter == null)
                {
                    return ApiResponseHelper.Fail("ニュースレターが見つかりません");
                }

                // ユーザー情報取得
                var user = await _newsletterRepository.GetUserByIdAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                // アイテム情報取得
                var item = await _newsletterRepository.GetItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                // メール送信処理
                // 注意: 実際のメール送信処理はここで実装する必要があります
                var mailContent = newsletter.MailBody
                    .Replace("{UserName}", user.Name)
                    .Replace("{ItemName}", item.ItemName ?? string.Empty)
                    .Replace("{ItemAmount}", item.ItemAmount.ToString());

                // TODO: 実際のメール送信処理を実装
                // 例: await _emailService.SendAsync(user.Email, newsletter.MailTitle, mailContent);

                return ApiResponseHelper.Success<object>(null, "ニュースレターを送信しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター送信中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}
