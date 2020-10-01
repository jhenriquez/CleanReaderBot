using System;
using System.Linq;
using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.SearchForBooks;
using CleanReaderBot.Webhooks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace CleanReaderBot.Webhooks.Services {
  public class TelegramBotService : ITelegramBotService {
    public ITelegramBotClient Client { get; }
    public TelegramSettings Settings { get; }
    public ILogger<TelegramBotService> Logger { get; }

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
      var inlineQueryArticleResults = result.Items.Select ((b) => CreateInlineQueryResultArticle(b, CreateInputTextMessageContent)).ToList ();

      await Client.AnswerInlineQueryAsync (
        inlineQueryId: messageId,
        results: inlineQueryArticleResults
      );
    }

    public InputTextMessageContent CreateInputTextMessageContent (Book book) {
      return new InputTextMessageContent ($"<a href=\"{book.ImageUrl}\" target=\"_black\">&#8203;</a><b>{book.Title}</b>\nBy <a href=\"https://www.goodreads.com/author/show/{book.Author.Id}\">{book.Author.Name}</a>\n\nRead more about this book on <a href=\"https://www.goodreads.com/book/show/{book.Id}\">Goodreads</a>.");
    }

    public InlineQueryResultArticle CreateInlineQueryResultArticle (Book book, Func<Book, InputMessageContentBase> createInputMessageContent) {
      var inputMessageContent = createInputMessageContent (book);
      return new InlineQueryResultArticle (book.Id.ToString (), book.Title, inputMessageContent) {
        Description = book.Author.Name,
          ThumbUrl = book.SmallImageUrl
      };
    }
  }
}