using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types.InlineQueryResults;

namespace ReaderBot.Webhooks.Tests.Comparers
{
    public class InlineQueryResultArticleComparer : IEqualityComparer<InlineQueryResultArticle>
    {
        public bool Equals([AllowNull] InlineQueryResultArticle x, [AllowNull] InlineQueryResultArticle y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode([DisallowNull] InlineQueryResultArticle obj)
        {
            return int.Parse(obj.Id);
        }
    }
}