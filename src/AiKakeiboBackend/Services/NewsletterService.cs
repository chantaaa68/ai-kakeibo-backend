using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    [Service]
    public class NewsletterService : INewsletterService
    {
        private readonly INewsletterRepository _newsletterRepository;

        public NewsletterService(INewsletterRepository newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        /// <summary>
        /// 新規ニュースレターテンプレートを登録します。
        /// メールタイトルと本文を含むテンプレートを作成し、後でユーザーへのメール送信に利用します。
        /// </summary>
        /// <param name="request">ニュースレター登録リクエスト（タイトル、メール本文を含む）</param>
        /// <returns>登録成功時は登録件数を含むApiResponse。登録失敗時はエラーメッセージを返却</returns>
        public async Task<IActionResult> RegistNewsletterAsync(RigistNewsletterRequest request)
        {
            try
            {
                NewsletterTemplate newsletter = new NewsletterTemplate
                {
                    MailTitle = request.Title ?? string.Empty,
                    MailBody = request.MailBody ?? string.Empty
                };

                await _newsletterRepository.CreateNewsletterAsync(newsletter);

                RigistNewsletterResponse response = new RigistNewsletterResponse
                {
                    Count = 1
                };

                return ApiResponseHelper.Success(response, "ニュースレターを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター登録中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 既存のニュースレターテンプレートを更新します。
        /// メールタイトルと本文を更新対象とします。
        /// </summary>
        /// <param name="request">ニュースレター更新リクエスト（ニュースレターID、更新後のタイトル、メール本文を含む）</param>
        /// <returns>更新成功時は更新件数を含むApiResponse。ニュースレターが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> UpdateNewsletterAsync(UpdateNewsletterRequest request)
        {
            try
            {
                NewsletterTemplate? newsletter = await _newsletterRepository.GetByIdAsync(request.Id);

                if (newsletter == null)
                {
                    return ApiResponseHelper.Fail("ニュースレターが見つかりません");
                }

                newsletter.MailTitle = request.Title ?? string.Empty;
                newsletter.MailBody = request.MailBody ?? string.Empty;

                await _newsletterRepository.UpdateNewsletterAsync(newsletter);

                UpdateNewsletterResponse response = new UpdateNewsletterResponse
                {
                    Count = 1
                };

                return ApiResponseHelper.Success(response, "ニュースレターを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター更新中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// ニュースレターテンプレートを使用してユーザーにメールを送信します。
        /// テンプレート内のプレースホルダー（{UserName}、{ItemName}、{ItemAmount}）を実際の値に置換して送信します。
        /// 注意：現在はメール送信処理の実装が未完了です（TODO参照）。
        /// </summary>
        /// <param name="request">ニュースレター送信リクエスト（ニュースレターID、ユーザーID、アイテムIDを含む）</param>
        /// <returns>送信成功時は送信件数を含むApiResponse。ニュースレター、ユーザー、アイテムが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> SendMailAsync(SendNewsletterRequest request)
        {
            try
            {
                // ニュースレターテンプレート取得
                NewsletterTemplate? newsletter = await _newsletterRepository.GetByIdAsync(request.NewsletterId);

                if (newsletter == null)
                {
                    return ApiResponseHelper.Fail("ニュースレターが見つかりません");
                }

                // ユーザー情報取得
                Users? user = await _newsletterRepository.GetUserByIdAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                // アイテム情報取得
                KakeiboItem? item = await _newsletterRepository.GetItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                // メール送信処理
                // 注意: 実際のメール送信処理はここで実装する必要があります
                string mailContent = newsletter.MailBody
                    .Replace("{UserName}", user.Name)
                    .Replace("{ItemName}", item.ItemName ?? string.Empty)
                    .Replace("{ItemAmount}", item.ItemAmount.ToString());

                // TODO: 実際のメール送信処理を実装
                // 例: await _emailService.SendAsync(user.Email, newsletter.MailTitle, mailContent);

                SendNewsletterResponse response = new SendNewsletterResponse
                {
                    SendCount = 1
                };

                return ApiResponseHelper.Success(response, "ニュースレターを送信しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ニュースレター送信中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}
