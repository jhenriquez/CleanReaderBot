using System;
using System.Threading.Tasks;
using ReaderBot.Application.Common.Interfaces;

namespace ReaderBot.Application.GetBookInformation {
    public class GetBook : IMessage<GetBookResult> {
        public string BookId { get; }
        public GetBook (string id) {
            if (string.IsNullOrEmpty (id)) {
                throw new ArgumentException ("Id is required.");
            }

            BookId = id;
        }

        public class Handler : IHandler<GetBook, GetBookResult>
        {
            private IBookProvider BookProvider { get; }

            public Handler(IBookProvider bookProvider)
            {
                if (bookProvider == null) {
                    throw new ArgumentNullException("A book provider is required. The provided value is null.");
                }

                BookProvider = bookProvider;
            }

            public async Task<GetBookResult> Execute(GetBook query)
            {
                var book = await BookProvider.GetBook(query);
                return new GetBookResult(book);
            }
        }
    }
}