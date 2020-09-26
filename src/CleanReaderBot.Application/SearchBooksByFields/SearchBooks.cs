using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Interfaces;

namespace CleanReaderBot.Application.SearchBooksByFields
{
  public class SearchBooks : IQuery<SearchBooksResult>
  {
    public string Query { get; private set; }

    public SearchableFields Field { get; private set; }

    public SearchBooks(string query = "", SearchableFields field = SearchableFields.All) {
      this.Query = query == null ? "" : query;
      this.Field = field;
    }
    
    public class Handler : IHandler<SearchBooks, SearchBooksResult>
    {
      private readonly IBookProvider provider;

      public Handler(IBookProvider provider)
      {
        this.provider = provider;
      }

      public async Task<SearchBooksResult> Execute(SearchBooks query)
      {
          var books = await provider.Search(query);
          return SearchBooksResult.For(books);
      }
    }
  }
}