#nullable enable

using ReaderBot.Application.Common.Entities;

namespace ReaderBot.Application.GetBookInformation
{
    public class GetBookResult
    {
        public GetBookResult(Book? book)
        {
            Book = book;
        }

        public Book? Book { get; private set; }
    }
}