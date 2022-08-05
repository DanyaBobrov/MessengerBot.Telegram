using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerBot.Telegram
{
    internal class TelegramBotHealthCheck : IHealthCheck
    {
        private readonly ITelegramBot telegramBot;

        public TelegramBotHealthCheck(ITelegramBot telegramBot)
        {
            this.telegramBot = telegramBot;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await telegramBot.GetMeAsync();
                return HealthCheckResult.Healthy("Ok");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Error", ex);
            }
        }
    }
}