using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.SearchForBooks;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Webhooks.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;
using Xunit;

namespace CleanReaderBot.Webhooks.Tests.Services {
    public class TelegramBotServiceTest {
        private ITelegramBotClient TelegramBotClient;
        private IOptions<TelegramSettings> TelegramSettings;
        private ILogger<TelegramBotService> TelegramBotServiceLogger;

        private IBotService TelegramBotService;

        public TelegramBotServiceTest () {
            TelegramSettings = Options.Create<TelegramSettings> (new TelegramSettings {
                Token = "SomeToken",
                    WebhookUrl = "SomeWebhookUrl"
            });

            TelegramBotClient = Substitute.For<ITelegramBotClient> ();

            TelegramBotServiceLogger = Substitute.For<ILogger<TelegramBotService>> ();

            TelegramBotService = new TelegramBotService (TelegramBotClient, TelegramSettings, TelegramBotServiceLogger);
        }

        [Fact]
        public async Task TelegramBotService__StartWebhook__Uses_SetWebhookAsync_With_The_Given_WebhookUrl () {
            await TelegramBotService.StartWebhook ();
            await TelegramBotClient.Received ().SetWebhookAsync (TelegramSettings.Value.WebhookUrl);
        }

        [Fact]
        public async Task TelegramBotService__SendSearchResults__Uses_AnswerInlineQueryAsync__With_Items_As_InlineQueryResultArticles () {
            var rand = new Random ();
            
            var booksSearchResult = SearchBooksResult.For (new Book[] {
                new Book () {
                    Id = rand.Next (10000),
                    Title = "Probably a book that does not exist in real life."
                }
            });

            var inlineQueryResults = booksSearchResult.Items.Select((b) => 
                new InlineQueryResultArticle(
                    id: b.Id.ToString(),
                    title: b.Title,
                    inputMessageContent: new InputTextMessageContent(String.Empty)
                )
            ).ToList();

            var inlineQueryId = "SomeFakeId";

            await TelegramBotService.SendSearchResults (booksSearchResult, inlineQueryId);

            /*
             TODO:
             This does not verify the list of articles is the expected one.
             Need a way to verify equality of two InlineQueryResultArticle objects.
            */
            await TelegramBotClient.Received().AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryId,
                results: Arg.Any<IList<InlineQueryResultArticle>>()
            );
        }
    }
}