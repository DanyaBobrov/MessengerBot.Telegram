using MessengerBot.Telegram;
using MessengerBot.Telegram.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration, Action<TelegramBotBuilder> action = null)
        {
            action?.Invoke(new TelegramBotBuilder(services));

            services
                .Configure<TelegramConfig>(configuration)
                .AddHttpClient<ITelegramBot, TelegramBot>((client) =>
                {
                    client.BaseAddress = new Uri("https://api.telegram.org");
                });
            return services;
        }
    }
}