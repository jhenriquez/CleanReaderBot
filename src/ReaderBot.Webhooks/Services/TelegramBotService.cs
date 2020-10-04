using System;
using System.Linq;
using System.Threading.Tasks;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace ReaderBot.Webhooks.Services {
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
      var bookInfoHTML = $"<a href=\"{book.ImageUrl}\">&#8205;</a><b>{book.Title}</b>\nBy {String.Join(", ", book.Authors.Select((author) => $"<a href=\"https://www.goodreads.com/author/show/{author.Id}\">{author.Name}</a>"))}\n&#127775 <b>{String.Format("{0:0.00}",book.AverageRating)}</b>\n\nRead more about this book on <a href=\"https://www.goodreads.com/book/show/{book.Id}\">Goodreads</a>.";
      var inputTextMessageContent = new InputTextMessageContent (bookInfoHTML);
      inputTextMessageContent.ParseMode = ParseMode.Html;
      return inputTextMessageContent;
    }

    public InlineQueryResultArticle CreateInlineQueryResultArticle (Book book, Func<Book, InputMessageContentBase> createInputMessageContent) {
      var inputMessageContent = createInputMessageContent (book);
      return new InlineQueryResultArticle (book.Id.ToString (), book.Title, inputMessageContent) {
        Description = String.Join(", ", book.Authors.ToList()),
          ThumbUrl = book.SmallImageUrl
      };
    }
  }
}