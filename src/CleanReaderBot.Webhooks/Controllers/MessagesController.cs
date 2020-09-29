using System.Threading.Tasks;
using CleanReaderBot.Webhooks.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using CleanReaderBot.Webhooks.Models;

namespace CleanReaderBot.Webhooks.Controllers
{
  public class MessagesController : Controller
  {
    private readonly ISpecificBotService<TelegramBotClient, TelegramSettings> telegram;

    public MessagesController(ISpecificBotService<TelegramBotClient, TelegramSettings> telegram)
    {
      this.telegram = telegram;
    }

    [HttpPost]
    public async Task<IActionResult> Telegram([FromBody] Update message)
    {
      return Ok();
    }
  }
}