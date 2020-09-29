using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace CleanReaderBot.Webhooks.Services
{
  public class TelegramBotService : ISpecificBotService<TelegramBotClient, TelegramSettings>
  {
    public TelegramBotClient Client { get; }
    public TelegramSettings Settings { get; }

    public TelegramBotService(IOptions<TelegramSettings> config) {
        Settings = config.Value;
        Client = new TelegramBotClient(Settings.Token);
    }

    public async Task StartWebHook()
    {
      await Client.SetWebhookAsync(Settings.WebhookUrl);
    }
  }
}