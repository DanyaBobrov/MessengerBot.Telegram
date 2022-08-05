using Microsoft.Extensions.DependencyInjection;

namespace MessengerBot.Telegram.Models
{
    public class TelegramBotBuilder
    {
        private readonly IServiceCollection service;

        public TelegramBotBuilder(IServiceCollection service)
        {
            this.service = service;
        }

        public void AddHealthCheck(string name = null)
        {
            service
                .AddHealthChecks()
                .AddCheck<TelegramBotHealthCheck>(name ?? $"{nameof(MessengerBot.Telegram)}");
        }
    }
}