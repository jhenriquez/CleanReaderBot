using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;
using ReaderBot.Webhooks.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

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

    public virtual async Task SendSearchResults (SearchBooksResult result, string refMessageId) {
      var inlineQueryArticleResults = result.Items.Select ((b) => CreateInlineQueryResultArticle (b, CreateInputTextMessageContent)).ToList ();

      await Client.AnswerInlineQueryAsync (
        inlineQueryId: refMessageId,
        results: inlineQueryArticleResults
      );
    }

    public string GenerateBookMarkup (Book book) {
      var brElementRegExp = new Regex(@"<br\s+?/>", RegexOptions.IgnoreCase);
      var bookDescription = brElementRegExp.Replace(book.Description, new MatchEvaluator((_) => "\n"));

      return $"<a href=\"{book.ImageUrl}\">&#8205;</a><b>{book.Title}</b>\nBy {String.Join(", ", book.Authors.Select((author) => $"<a href=\"https://www.goodreads.com/author/show/{author.Id}\">{author.Name}</a>"))}\n&#127775<b>{String.Format ("{0:0.00}",book.AverageRating)}</b>{(String.IsNullOrEmpty(bookDescription) ? "" : $"\n\n{bookDescription}")}\n\nRead more about this book on <a href=\"https://www.goodreads.com/book/show/{book.Id}\">Goodreads</a>.";
    }

    public InputTextMessageContent CreateInputTextMessageContent (Book book) {
      var bookInfoHTML = GenerateBookMarkup(book);
      var inputTextMessageContent = new InputTextMessageContent (bookInfoHTML);
      inputTextMessageContent.ParseMode = ParseMode.Html;
      return inputTextMessageContent;
    }

    public InlineQueryResultArticle CreateInlineQueryResultArticle (Book book, Func<Book, InputMessageContentBase> createInputMessageContent) {
      var inputMessageContent = createInputMessageContent (book);
      return new InlineQueryResultArticle (book.Id.ToString (), book.Title, inputMessageContent) {
        Description = String.Join (", ", book.Authors.ToList ()),
          ThumbUrl = book.SmallImageUrl,
          ReplyMarkup = new InlineKeyboardMarkup (new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData ("Read more", book.Id) })
      };
    }

    public async Task SendGetBookResult (GetBookResult getBookResult, string refMessageId) {
      if (getBookResult.Book == null) {
        throw new ArgumentNullException("The book provided within the getBookResult can not be null.");
      }

      await Client.EditMessageTextAsync (
        inlineMessageId: refMessageId,
        text: GenerateBookMarkup(getBookResult.Book),
        parseMode: ParseMode.Html
      );
    }
  }
}