using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using CleanReaderBot.Webhooks.Services;
using CleanReaderBot.Webhooks.Models;

namespace CleanReaderBot.Webhooks
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBotWebhooksTelegramIntegration(this IServiceCollection services)
    {
      services.AddSingleton<ISpecificBotService<TelegramBotClient, TelegramSettings>, TelegramBotService>();
      return services;
    }
  }
}