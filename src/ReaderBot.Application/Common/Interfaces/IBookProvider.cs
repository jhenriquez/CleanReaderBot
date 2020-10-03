using System.Threading.Tasks;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Application.Common.Interfaces
{
  public interface IBookProvider
  {
    Task<Book[]> Search(SearchBooks query);
  }
}