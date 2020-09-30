using System.Threading.Tasks;
using CleanReaderBot.Webhooks.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchForBooks;

namespace CleanReaderBot.Webhooks.Controllers
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
      var searchBooksQuery = new SearchBooks(message.InlineQuery.Query);
      var booksSearchResult = await SearchBooksHandler.Execute(searchBooksQuery);
      await TelegramService.SendSearchResults(booksSearchResult, message.InlineQuery.Id);
      return Ok();
    }
  }
}