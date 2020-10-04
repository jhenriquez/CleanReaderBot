using System;
using System.Web;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Application.BookProvider.Goodreads {
    public static class GoodreadsUriBuilder {
        public static UriBuilder BuildFor (SearchBooks query, GoodreadsAPISettings settings) {
            var uri = new UriBuilder ("https://www.goodreads.com/search/index.xml");
            var queryParameters = HttpUtility.ParseQueryString (string.Empty);
            queryParameters["key"] = settings.Key;
            queryParameters["q"] = query.Query;
            queryParameters["field"] = query.Field.ToString ();
            uri.Query = queryParameters.ToString ();
            return uri;
        }

        public static UriBuilder BuildFor(GetBook getBookQuery, GoodreadsAPISettings settings) =>
            new UriBuilder($"https://www.goodreads.com/book/show/{getBookQuery.BookId}.xml?key={settings.Key}");
    }
}