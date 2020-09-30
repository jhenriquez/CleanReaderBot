using System;
using System.Linq;
using System.Threading.Tasks;
using CleanReaderBot.Application.SearchForBooks;
using CleanReaderBot.Webhooks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace CleanReaderBot.Webhooks.Services {
  public class TelegramBotService : ISpecificBotService<ITelegramBotClient, TelegramSettings> {
    public ITelegramBotClient Client { get; }
    public TelegramSettings Settings { get; }
    public ILogger<TelegramBotService> Logger { get; }
    public int Id;

    public TelegramBotService (ITelegramBotClient client, IOptions<TelegramSettings> config, ILogger<TelegramBotService> logger) {
      Settings = config.Value;
      Client = client;
      Logger = logger;
    }

    public virtual async Task StartWebhook () {
      Logger.LogInformation ($"Setting Telegram webhook to: \"{Settings.WebhookUrl}\"");
      await Client.SetWebhookAsync (Settings.WebhookUrl);
    }

    public virtual async Task SendSearchResults (SearchBooksResult result, string messageId) {
      var inlineQueryArticleResults = result.Items.Select ((b) =>
        new InlineQueryResultArticle (
          id: b.Id.ToString (),
          title: b.Title,
          inputMessageContent: new InputTextMessageContent (String.Empty)
        )
      ).ToList();

      await Client.AnswerInlineQueryAsync(
        inlineQueryId: messageId,
        results: inlineQueryArticleResults
      );
    }
  }
}