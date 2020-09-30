using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using CleanReaderBot.Application.SearchForBooks;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Webhooks.Services;

namespace CleanReaderBot.Webhooks.IntegrationTests.Services
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