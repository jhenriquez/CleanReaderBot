using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using ReaderBot.Webhooks.Services;
using ReaderBot.Webhooks.Models;
using Microsoft.Extensions.Options;

namespace ReaderBot.Webhooks
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddReaderBotWebhooksIntegration(this IServiceCollection services)
    {      
      services.AddSingleton<ITelegramBotClient>(x =>
      {
        var TelegramSettings = x.GetRequiredService<IOptions<TelegramSettings>>();
        return new TelegramBotClient(TelegramSettings.Value.Token);
      });

      services.AddSingleton<TelegramBotService>();
      services.AddSingleton<IBotService>(x => x.GetRequiredService<TelegramBotService>());
      services.AddSingleton<ISpecificBotService<ITelegramBotClient, TelegramSettings>>(x => x.GetRequiredService<TelegramBotService>());
      return services;
    }
  }
}