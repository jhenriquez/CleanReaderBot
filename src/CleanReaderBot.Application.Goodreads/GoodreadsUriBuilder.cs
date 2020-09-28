using System;
using System.Web;
using CleanReaderBot.Application.SearchBooksByFields;

namespace CleanReaderBot.Application.Goodreads {
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
    }
}