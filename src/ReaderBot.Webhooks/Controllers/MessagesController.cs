using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Models;
using ReaderBot.Webhooks.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ReaderBot.Webhooks.Controllers {
  public class MessagesController : Controller {

    public IHandler<SearchBooks, SearchBooksResult> SearchBooksHandler { get; }
    public IHandler<GetBook, GetBookResult> GetBookHandler { get; }
    public ISpecificBotService<ITelegramBotClient, TelegramSettings> TelegramService { get; }
    public MessagesController (
      IHandler<SearchBooks, SearchBooksResult> searchBooksHandler,
      IHandler<GetBook, GetBookResult> getBookHandler,
      ISpecificBotService<ITelegramBotClient, TelegramSettings> telegram) {
      SearchBooksHandler = searchBooksHandler;
      GetBookHandler = getBookHandler;
      TelegramService = telegram;
    }

    [HttpPost]
    public async Task<IActionResult> Telegram ([FromBody] Update message) {
      switch (message.Type) {
        case UpdateType.InlineQuery:
          var searchBooksQuery = new SearchBooks (message.InlineQuery.Query);
          var booksSearchResult = await SearchBooksHandler.Execute (searchBooksQuery);
          await TelegramService.SendSearchResults (booksSearchResult, message.InlineQuery.Id);
          break;
        case UpdateType.CallbackQuery:
          var getBookQuery = new GetBook(message.CallbackQuery.Data);
          var getBookResult = await GetBookHandler.Execute(getBookQuery);
          await TelegramService.SendGetBookResult(getBookResult, message.CallbackQuery.InlineMessageId);
          break;
      }

      return Ok ();
    }
  }
}