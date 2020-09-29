using System.Threading.Tasks;
using CleanReaderBot.Webhooks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace CleanReaderBot.Webhooks.Services {
  public class TelegramBotService : ISpecificBotService<TelegramBotClient, TelegramSettings> {
    public TelegramBotClient Client { get; }
    public TelegramSettings Settings { get; }
    public ILogger<TelegramBotService> Logger { get; }
    public int Id;

    public TelegramBotService (IOptions<TelegramSettings> config, ILogger<TelegramBotService> logger) {
      Id = new System.Random().Next(0, 1000);
      Settings = config.Value;
      Client = new TelegramBotClient (Settings.Token);
      Logger = logger;
    }

    public virtual async Task StartWebHook () {
      Logger.LogInformation($"Setting Telegram webhook to: \"{Settings.WebhookUrl}\"");
      await Client.SetWebhookAsync (Settings.WebhookUrl);
    }
  }
}