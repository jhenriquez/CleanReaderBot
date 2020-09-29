using System.Collections.Generic;
using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Interfaces;

namespace CleanReaderBot.Application.SearchBooksByFields {
  public class SearchBooks : IMessage<SearchBooksResult> {
    public string Query { get; private set; }

    public SearchableFields Field { get; private set; }

    public SearchBooks (string query = "", SearchableFields field = SearchableFields.All) {
      this.Query = query == null ? "" : query;
      this.Field = field;
    }

    public override bool Equals (object obj) {
      return obj is SearchBooks books &&
        Query == books.Query &&
        Field == books.Field;
    }

    public override int GetHashCode()
    {
      int hashCode = -1960062974;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Query);
      hashCode = hashCode * -1521134295 + Field.GetHashCode();
      return hashCode;
    }

    public class Handler : IHandler<SearchBooks, SearchBooksResult> {
      private readonly IBookProvider provider;

      public Handler (IBookProvider provider) {
        this.provider = provider;
      }

      public async Task<SearchBooksResult> Execute (SearchBooks query) {
        var books = await provider.Search (query);
        return SearchBooksResult.For (books);
      }
    }
  }
}