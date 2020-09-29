using System.Threading.Tasks;
using CleanReaderBot.Webhooks.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using CleanReaderBot.Webhooks.Models;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;

namespace CleanReaderBot.Webhooks.Controllers
{
  public class MessagesController : Controller
  {
    public MessagesController(
      IHandler<SearchBooks, SearchBooksResult> searchBooksHandler,
      ISpecificBotService<TelegramBotClient, TelegramSettings> telegram)
    {
      SearchBooksHandler = searchBooksHandler;
      TelegramService = telegram;
    }

    public IHandler<SearchBooks, SearchBooksResult> SearchBooksHandler { get; }
    public ISpecificBotService<TelegramBotClient, TelegramSettings> TelegramService { get; }

    [HttpPost]
    public async Task<IActionResult> Telegram([FromBody] Update message)
    {
      var searchBooksQuery = new SearchBooks(message.InlineQuery.Query);
      await SearchBooksHandler.Execute(searchBooksQuery);
      return Ok();
    }
  }
}