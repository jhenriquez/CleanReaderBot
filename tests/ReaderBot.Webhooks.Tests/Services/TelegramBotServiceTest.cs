using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Models;
using ReaderBot.Webhooks.Services;
using ReaderBot.Webhooks.Tests.Comparers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Xunit;
using Telegram.Bot.Types.ReplyMarkups;
using ReaderBot.Application.GetBookInformation;

namespace ReaderBot.Webhooks.Tests.Services {
    public class TelegramBotServiceTest {
        private ITelegramBotClient TelegramBotClient;
        private IOptions<TelegramSettings> TelegramSettings;
        private ILogger<TelegramBotService> TelegramBotServiceLogger;

        private ITelegramBotService TelegramBotService;

        public TelegramBotServiceTest () {
            TelegramSettings = Options.Create<TelegramSettings> (new TelegramSettings {
                Token = "SomeToken",
                WebhookUrl = "SomeWebhookUrl"
            });

            TelegramBotClient = Substitute.For<ITelegramBotClient> ();

            TelegramBotServiceLogger = Substitute.For<ILogger<TelegramBotService>> ();

            TelegramBotService = new TelegramBotService (TelegramBotClient, TelegramSettings, TelegramBotServiceLogger);
        }

        private Book GetExampleBook () {
            return new Book () {
                Id = "375802",
                Title = "Ender's Game (Ender's Saga, #1)",
                AverageRating = 4.30,
                Authors = new Author[] { new Author { Id = "589", Name = "Orson Scott Card" } },
                Description = "Andrew \"Ender\" Wiggin thinks he is playing computer simulated war games; he is, in fact, engaged in something far more desperate. The result of genetic experimentation, Ender may be the military genius Earth desperately needs in a war against an alien enemy seeking to destroy all human life. The only way to find out is to throw Ender into ever harsher training, to chip away and find the diamond inside, or destroy him utterly. Ender Wiggin is six years old when it begins. He will grow up fast.<br /><br />But Ender is not the only result of the experiment. The war with the Buggers has been raging for a hundred years, and the quest for the perfect general has been underway almost as long. Ender's two older siblings, Peter and Valentine, are every bit as unusual as he is, but in very different ways. While Peter was too uncontrollably violent, Valentine very nearly lacks the capability for violence altogether. Neither was found suitable for the military's purpose. But they are driven by their jealousy of Ender, and by their inbred drive for power. Peter seeks to control the political process, to become a ruler. Valentine's abilities turn more toward the subtle control of the beliefs of commoner and elite alike, through powerfully convincing essays. Hiding their youth and identities behind the anonymity of the computer networks, these two begin working together to shape the destiny of Earth-an Earth that has no future at all if their brother Ender fails.<br /><br />Source: hatrack.com",
                ImageUrl = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY160_.jpg",
                SmallImageUrl = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY75_.jpg"
            };
        }

        [Fact]
        public async Task TelegramBotService__StartWebhook__Uses_SetWebhookAsync_With_The_Given_WebhookUrl () {
            await TelegramBotService.StartWebhook ();
            await TelegramBotClient.Received ().SetWebhookAsync (TelegramSettings.Value.WebhookUrl);
        }

        [Fact]
        public async Task TelegramBotService__SendSearchResults__Uses_AnswerInlineQueryAsync__With_Items_As_InlineQueryResultArticles () {
            var booksSearchResult = new SearchBooksResult(new Book[] { GetExampleBook() });
            var inlineQueryResults = booksSearchResult.Items.Select ((b) => TelegramBotService.CreateInlineQueryResultArticle(b, TelegramBotService.CreateInputTextMessageContent)).ToList ();
            var inlineQueryId = "SomeFakeId";

            await TelegramBotService.SendSearchResults (booksSearchResult, inlineQueryId);

            await TelegramBotClient.Received ().AnswerInlineQueryAsync (
                inlineQueryId: inlineQueryId,
                results: Arg.Is<IList<InlineQueryResultArticle>> (iqras => iqras.SequenceEqual(inlineQueryResults, new InlineQueryResultArticleComparer()))
            );
        }

        [Fact]
        public void TelegramBotService__CreateInputTextMessageContent__Creates_HTML_Content_When_Given_A_Book () {
            var book = GetExampleBook();
            var inputTextMessageContent = TelegramBotService.CreateInputTextMessageContent (book);

            inputTextMessageContent.MessageText.Should().Be(TelegramBotService.GenerateBookMarkup(book));
            inputTextMessageContent.ParseMode = ParseMode.Html;
        }

        [Fact]
        public void TelegramBotService__CreateInlineQueryResultArticle__Returns_A_Valid_Article_Given_A_Book () {
            var book = GetExampleBook();
            var createInputMessageContent = Substitute.For<Func<Book, InputMessageContentBase>>();
            var inlineQueryResultArticle = TelegramBotService.CreateInlineQueryResultArticle(book, createInputMessageContent);

            var inlineKeyboardMarkup = new InlineKeyboardMarkup(
                inlineKeyboardRow: new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Read more", book.Id) }
            );

            inlineQueryResultArticle.Title.Should().Be(book.Title);
            inlineQueryResultArticle.Description.Should().Be(String.Join(", ", book.Authors.ToList()));
            inlineQueryResultArticle.ThumbUrl.Should().Be(book.SmallImageUrl);
            inlineQueryResultArticle.ReplyMarkup.Should().BeEquivalentTo(inlineKeyboardMarkup);
            createInputMessageContent.ReceivedCalls();
        }

        [Fact]
        public async Task TelegramBotService__SendGetBookResult__Uses_EditMessageText_With_Book_MarkupAsync() {
            var getBookResult = new GetBookResult(GetExampleBook());
            var refMessageId = "SomeFakeId";

            await TelegramBotService.SendGetBookResult (getBookResult, refMessageId);

            await TelegramBotClient.Received ().EditMessageTextAsync (
                inlineMessageId: refMessageId,
                text: TelegramBotService.GenerateBookMarkup(GetExampleBook()),
                parseMode: ParseMode.Html
            );
        }

        [Fact]
        public void TelegramBotService__SendGetBookResult__Throws_ArgumentNullException_If_Book_Is_Not_Provided() {
            var getBookResult = new GetBookResult(null);
            var refMessageId = "SomeFakeId";

            this.Invoking(async (_) => await TelegramBotService.SendGetBookResult (getBookResult, refMessageId))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TelegramBotService__GenerateBookMarkup__Replaces_Br_Tags_With_Carriage_Returns () {
            var book = GetExampleBook();
            var brElementRegExp = @"<br\s+?/>";
            book.Description.Should().MatchRegex(brElementRegExp);
            var bookMarkup = TelegramBotService.GenerateBookMarkup(book);
            bookMarkup.Should().NotMatchRegex(brElementRegExp);
        }
    }
}