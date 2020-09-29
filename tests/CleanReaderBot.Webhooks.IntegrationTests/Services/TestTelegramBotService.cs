using System.Threading.Tasks;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Webhooks.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanReaderBot.Webhooks.IntegrationTests.Services
{
    public class TestTelegramBotService : TelegramBotService
    {
        public TestTelegramBotService(IOptions<TelegramSettings> config, ILogger<TelegramBotService> logger)
            : base(config, logger) { }

        public override async Task StartWebHook () {
            // We're just overriding to prevent the real thing during tests.
        }
    }
}