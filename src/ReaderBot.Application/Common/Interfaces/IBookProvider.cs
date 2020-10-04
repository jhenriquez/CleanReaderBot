
#nullable enable

using System.Threading.Tasks;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Application.Common.Interfaces
{
  public interface IBookProvider
  {
    Task<Book[]> Search(SearchBooks query);
    Task<Book?> GetBook(GetBook getBookQuery);
  }
}