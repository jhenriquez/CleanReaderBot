using System;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Webhooks.Models;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace ReaderBot.Webhooks.Services
{
    public interface ITelegramBotService : ISpecificBotService<ITelegramBotClient, TelegramSettings>
    {
        InputTextMessageContent CreateInputTextMessageContent(Book book);
        InlineQueryResultArticle CreateInlineQueryResultArticle(Book book, Func<Book, InputMessageContentBase> createInputMessageContent);
    }
}