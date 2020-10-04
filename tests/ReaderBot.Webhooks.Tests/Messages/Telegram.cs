using System;
using System.Threading.Tasks;
using NSubstitute;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Controllers;
using ReaderBot.Webhooks.Models;
using ReaderBot.Webhooks.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace ReaderBot.Webhooks.Tests.Messages {
    public class Telegram {
        private User GetExampleUser () {
            return new User {
                Id = 12345,
                    IsBot = false,
                    FirstName = "Tasty",
                    LastName = "Tester",
                    Username = "TestRunner"
            };
        }
        private Update GetExampleInlineQuery () {
            return new Update {
                Id = 123456,
                    InlineQuery = new InlineQuery {
                        Id = "1234567",
                        From = GetExampleUser (),
                        Query = "Sample Query",
                        Offset = ""
                        }
            };
        }

        private Update GetExampleCallbackQuery () {
            return new Update {
                Id = 1523545,
                    CallbackQuery = new CallbackQuery {
                        Id = "603668005804303499",
                        From = GetExampleUser (),
                        InlineMessageId = "AQAAAExXBQDaqGAIASaxuTUz0Z4",
                        ChatInstance = "-8417044987993467880",
                        Data = "15733851"
                        }
            };
        }

        private Book GetExampleBook () {
            return  new Book () {
                Id = "132434535",
                Title = "Probably a book that does not exist in real life.",
                Description = "Some book that we have just invented for the purpose of this test"
            };
        }

        [Fact]
        public async Task Messages__Telegram__Uses_SearchBooks_For_InlineQuery_Request () {
            var searchBooksHandler = Substitute.For<IHandler<SearchBooks, SearchBooksResult>> ();
            var getBookHandler = Substitute.For<IHandler<GetBook, GetBookResult>> ();
            var botService = Substitute.For<ISpecificBotService<ITelegramBotClient, TelegramSettings>> ();
            var updateMessage = GetExampleInlineQuery ();
            var expectedSearchBooksQuery = new SearchBooks (updateMessage.InlineQuery.Query);
            var messagesController = new MessagesController (searchBooksHandler, getBookHandler, botService);

            await messagesController.Telegram (updateMessage);

            await searchBooksHandler.Received ().Execute (expectedSearchBooksQuery);
        }

        [Fact]
        public async Task Messages__Telegram__Uses_BookProvider_GetBook_For_CallbackQuery_Request () {
            var searchBooksHandler = Substitute.For<IHandler<SearchBooks, SearchBooksResult>> ();
            var getBookHandler = Substitute.For<IHandler<GetBook, GetBookResult>> ();
            var botService = Substitute.For<ISpecificBotService<ITelegramBotClient, TelegramSettings>> ();
            var updateMessage = GetExampleCallbackQuery ();
            var expectedGetBookQuery = new GetBook (updateMessage.CallbackQuery.Data);
            var messagesController = new MessagesController (searchBooksHandler, getBookHandler, botService);

            await messagesController.Telegram (updateMessage);

            await getBookHandler.Received ().Execute (expectedGetBookQuery);
        }

        [Fact]
        public async Task Messages__Telegram__Uses_TelegramBotService__SendSearchResults__To_Respond_With_A_SearchBooksResult () {
            var searchBooksHandler = Substitute.For<IHandler<SearchBooks, SearchBooksResult>> ();
            var getBookHandler = Substitute.For<IHandler<GetBook, GetBookResult>> ();
            var botService = Substitute.For<ISpecificBotService<ITelegramBotClient, TelegramSettings>> ();

            var booksSearchResult = new SearchBooksResult (new Book[] { GetExampleBook() });

            var updateMessage = GetExampleInlineQuery ();

            var expectedSearchBooksQuery = new SearchBooks (updateMessage.InlineQuery.Query);

            searchBooksHandler.Execute (expectedSearchBooksQuery).Returns (booksSearchResult);

            var messagesController = new MessagesController (searchBooksHandler, getBookHandler, botService);

            await messagesController.Telegram (updateMessage);

            await botService.Received ().SendSearchResults (booksSearchResult, updateMessage.InlineQuery.Id);
        }
        
        [Fact]
        public async Task Messages__Telegram__Uses_TelegramBotService__UpdateMessage_To_Send_GetBookResult() {
            var searchBooksHandler = Substitute.For<IHandler<SearchBooks, SearchBooksResult>> ();
            var getBookHandler = Substitute.For<IHandler<GetBook, GetBookResult>> ();
            var botService = Substitute.For<ISpecificBotService<ITelegramBotClient, TelegramSettings>> ();
            var expectedGetBookResult = new GetBookResult(GetExampleBook());
            var updateMessage = GetExampleCallbackQuery ();
            var expectedGetBookQuery = new GetBook (updateMessage.CallbackQuery.Data);
            var messagesController = new MessagesController (searchBooksHandler, getBookHandler, botService);
            getBookHandler.Execute(expectedGetBookQuery).Returns(expectedGetBookResult);

            await messagesController.Telegram (updateMessage);
            
            await botService.Received ().SendGetBookResult (expectedGetBookResult, updateMessage.CallbackQuery.InlineMessageId);
        }
    }
}