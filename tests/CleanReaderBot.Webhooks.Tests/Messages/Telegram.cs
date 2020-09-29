using System;
using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchForBooks;
using CleanReaderBot.Webhooks.Controllers;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Webhooks.Services;
using NSubstitute;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace CleanReaderBot.Webhooks.Tests.Messages
{
    public class Telegram
    {
        [Fact]
        public async Task Messages__Telegram__Uses_SearchBooks_For_InlineQuery_Request() 
        {
            var searchBooksHandler = Substitute.For<IHandler<SearchBooks, SearchBooksResult>>();
            var telegramBotService = Substitute.For<ISpecificBotService<TelegramBotClient, TelegramSettings>>();

            var rand = new Random ();

            var updateMessage = new Update {
                Id = rand.Next (1000, 1500),
                InlineQuery = new InlineQuery {
                    Id = rand.Next (1000, 1500).ToString (),
                    From = new User {
                        Id = rand.Next (1000, 1500),
                        IsBot = false,
                        FirstName = rand.Next (1000, 1500).ToString (),
                        LastName = rand.Next (1000, 1500).ToString (),
                        Username = rand.Next (1000, 1500).ToString ()
                    },
                Query = "Sample Query",
                Offset = ""
                }
            };

            var expectedSearchBooksQuery = new SearchBooks(updateMessage.InlineQuery.Query);

            var messagesController = new MessagesController(searchBooksHandler, telegramBotService);

            await messagesController.Telegram(updateMessage);

            await searchBooksHandler.Received().Execute(expectedSearchBooksQuery);
        } 
    }
}