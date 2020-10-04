using ReaderBot.Application.Common.Entities;

namespace ReaderBot.Application.SearchForBooks
{
    public class SearchBooksResult
    {
        public Book[] Items { get; private set; }
        
        public SearchBooksResult (Book[] items) {
            Items = items;
        }
    }
}