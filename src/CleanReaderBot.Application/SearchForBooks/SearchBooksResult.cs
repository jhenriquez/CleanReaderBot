using CleanReaderBot.Application.Common.Entities;

namespace CleanReaderBot.Application.SearchForBooks
{
    public class SearchBooksResult
    {
        public Book[] Items { get; private set; }

        public static SearchBooksResult For(Book[] books)
        {
            return new SearchBooksResult {
                Items = books
            };
        }
    }
}