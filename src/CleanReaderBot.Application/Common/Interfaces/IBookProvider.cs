using System.Threading.Tasks;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.SearchBooksByFields;

namespace CleanReaderBot.Application.Common.Interfaces
{
  public interface IBookProvider
  {
    Task<Book[]> Search(SearchBooks query);
  }
}