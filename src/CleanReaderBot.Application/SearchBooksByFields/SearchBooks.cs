using System.Threading.Tasks;

namespace CleanReaderBot.Application.SearchBooksByFields
{
  public class SearchBooks
  {
    public string Query { get; private set; }

    public SearchableFields Field { get; private set; }

    public SearchBooks(string query = "", SearchableFields field = SearchableFields.All) {
      this.Query = query == null ? "" : query;
      this.Field = field;
    }
  }
}