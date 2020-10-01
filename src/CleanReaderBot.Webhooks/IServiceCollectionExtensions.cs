using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using CleanReaderBot.Webhooks.Services;
using CleanReaderBot.Webhooks.Models;
using Microsoft.Extensions.Options;

namespace CleanReaderBot.Webhooks
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBotWebhooksIntegration(this IServiceCollection services)
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