using System.Threading.Tasks;
using ReaderBot.Webhooks.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using ReaderBot.Webhooks.Models;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.SearchForBooks;
using Telegram.Bot.Types.Enums;

namespace ReaderBot.Webhooks.Controllers
{
  public class MessagesController : Controller
  {
    public MessagesController(
      IHandler<SearchBooks, SearchBooksResult> searchBooksHandler,
      ISpecificBotService<ITelegramBotClient, TelegramSettings> telegram)
    {
      SearchBooksHandler = searchBooksHandler;
      TelegramService = telegram;
    }

    public IHandler<SearchBooks, SearchBooksResult> SearchBooksHandler { get; }
    public ISpecificBotService<ITelegramBotClient, TelegramSettings> TelegramService { get; }

    [HttpPost]
    public async Task<IActionResult> Telegram([FromBody] Update message)
    {
      switch (message.Type) {
        case UpdateType.InlineQuery:
          var searchBooksQuery = new SearchBooks(message.InlineQuery.Query);
          var booksSearchResult = await SearchBooksHandler.Execute(searchBooksQuery);
          await TelegramService.SendSearchResults(booksSearchResult, message.InlineQuery.Id);
          break;
      }
      
      return Ok();
    }
  }
}