using MessengerBot.Telegram.Exceptions;
using MessengerBot.Telegram.Helpers;
using MessengerBot.Telegram.Models;
using MessengerBot.Telegram.Models.Telegram;
using MessengerBot.Telegram.Models.Telegram.Internal;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessengerBot.Telegram
{
    internal class TelegramBot : ITelegramBot
    {
        private readonly HttpClient httpClient;
        private readonly IOptionsMonitor<TelegramConfig> telegramConfig;

        public TelegramBot(
            HttpClient httpClient,
            IOptionsMonitor<TelegramConfig> telegramConfig)
        {
            this.httpClient = httpClient;
            this.telegramConfig = telegramConfig;
        }

        public async Task<User> GetMeAsync()
        {
            using (var response = await httpClient.GetAsync($"/bot{telegramConfig.CurrentValue.Token}/getMe"))
            {
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = responseContent.FromJsonString<BaseResponse<User>>();
                if (result.Success)
                    return result.Result;

                throw new TelegramBotException($"Запрос завершился с ошибкой '{result.ErrorCode}'. Описание: {result.Description}");
            }
        }

        public Task<Message> SendAsync(MessageType messageType, MessageInfo message) => messageType switch
        {
            MessageType.Text => SendMessageAsync(message),
            MessageType.Photo => SendPhotoAsync(message),
            MessageType.Document => SendDocumentAsync(message),
            _ => throw new InvalidEnumArgumentException()
        };

        public async Task<Message> SendMessageAsync(MessageInfo message)
        {
            if (string.IsNullOrEmpty(message.Text))
                throw new ArgumentException($"Поле {nameof(message.Text)} не может быть пустым");

            var request = new MessageRequest()
            {
                ChatId = message.ChatId,
                Text = message.Text
            };

            using (var content = new StringContent(request.ToJsonString(), Encoding.UTF8, "application/json"))
            {
                using (var response = await httpClient.PostAsync($"/bot{telegramConfig.CurrentValue.Token}/sendMessage", content))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = responseContent.FromJsonString<BaseResponse<Message>>();
                    if (result.Success)
                        return result.Result;

                    throw new TelegramBotException($"Запрос завершился с ошибкой '{result.ErrorCode}'. Описание: {result.Description}");
                }
            }
        }

        public async Task<Message> SendPhotoAsync(MessageInfo message)
        {
            if (string.IsNullOrEmpty(message.FileName))
                throw new ArgumentException($"Поле {nameof(message.FileName)} не может быть пустым");
            if (message.File == null)
                throw new ArgumentException($"Поле {nameof(message.File)} не может быть пустым");

            using var streamcontent = new StreamContent(message.File);
            using var stringContent = new StringContent(message.ChatId);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(streamcontent, "photo", message.FileName);
                content.Add(stringContent, "chat_id");

                using (var response = await httpClient.PostAsync($"/bot{telegramConfig.CurrentValue.Token}/sendPhoto", content))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = responseContent.FromJsonString<BaseResponse<Message>>();
                    if (result.Success)
                        return result.Result;

                    throw new TelegramBotException($"Запрос завершился с ошибкой '{result.ErrorCode}'. Описание: {result.Description}");
                }
            }
        }

        public async Task<Message> SendDocumentAsync(MessageInfo message)
        {
            if (string.IsNullOrEmpty(message.FileName))
                throw new ArgumentException($"Поле {nameof(message.FileName)} не может быть пустым");
            if (message.File == null)
                throw new ArgumentException($"Поле {nameof(message.File)} не может быть пустым");

            using var streamcontent = new StreamContent(message.File);
            using var stringContent = new StringContent(message.ChatId);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(streamcontent, "document", message.FileName);
                content.Add(stringContent, "chat_id");

                using (var response = await httpClient.PostAsync($"/bot{telegramConfig.CurrentValue.Token}/sendDocument", content))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = responseContent.FromJsonString<BaseResponse<Message>>();
                    if (result.Success)
                        return result.Result;

                    throw new TelegramBotException($"Запрос завершился с ошибкой '{result.ErrorCode}'. Описание: {result.Description}");
                }
            }
        }
    }
}