using System;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Webhooks.Models;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace CleanReaderBot.Webhooks.Services
{
    public interface ITelegramBotService : ISpecificBotService<ITelegramBotClient, TelegramSettings>
    {
        InputTextMessageContent CreateInputTextMessageContent(Book book);
        InlineQueryResultArticle CreateInlineQueryResultArticle(Book book, Func<Book, InputMessageContentBase> createInputMessageContent);
    }
}