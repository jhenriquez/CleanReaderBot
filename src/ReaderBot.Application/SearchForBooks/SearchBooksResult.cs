using ReaderBot.Application.Common.Entities;

namespace ReaderBot.Application.SearchForBooks
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