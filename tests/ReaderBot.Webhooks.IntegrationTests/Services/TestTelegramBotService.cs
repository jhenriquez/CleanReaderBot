using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Models;
using ReaderBot.Webhooks.Services;

namespace ReaderBot.Webhooks.IntegrationTests.Services
{
    public class TestTelegramBotService : TelegramBotService
    {
        public TestTelegramBotService(ITelegramBotClient client, IOptions<TelegramSettings> config, ILogger<TelegramBotService> logger)
            : base(client, config, logger) { }

        public override async Task StartWebhook () {
            await Task.CompletedTask;
        }

        public override async Task SendSearchResults (SearchBooksResult result, string messageId = default) {
            await Task.CompletedTask;
        }
    }
}