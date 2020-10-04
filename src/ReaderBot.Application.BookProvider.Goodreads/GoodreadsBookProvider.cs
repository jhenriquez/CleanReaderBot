#nullable enable

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Application.BookProvider.Goodreads {
  public class GoodreadsBookProvider : IBookProvider {
    private HttpClient Client { get; }
    private GoodreadsAPISettings Settings { get; }

    public GoodreadsBookProvider (HttpClient client, IOptions<GoodreadsAPISettings> settings) {
      if (String.IsNullOrEmpty (settings.Value.Key)) {
        throw new ArgumentException ("The Goodreads API Key is required.");
      }

      Client = client;
      Settings = settings.Value;
    }

    public async Task<Book?> GetBook (GetBook getBookQuery) {
      try {
        var uri = GoodreadsUriBuilder.BuildFor (getBookQuery, Settings);
        var responseStream = await Client.GetStreamAsync (uri.ToString ());
        var xmlSerializer = new XmlSerializer (typeof (GoodreadsResponse));
        var response = (GoodreadsResponse) xmlSerializer.Deserialize (responseStream);
        var book = response.Book;

        return new Book {
          Id = book.Id.ToString (),
          AverageRating = book.AverageRating,
          Title = book.Title,
          Description = book.Description,
          Authors = book.Authors.Select((author) => new Author { Id = author.Id.ToString(), Name = author.Name } ).ToArray(),
          ImageUrl = book.ImageUrl,
          SmallImageUrl = book.SmallImageUrl
        };
      } catch {
        return null;
      }
    }

    public async Task<Book[]> Search (SearchBooks query) {
      var uri = GoodreadsUriBuilder.BuildFor (query, Settings);
      var responseStream = await Client.GetStreamAsync (uri.ToString ());
      var xmlSerializer = new XmlSerializer (typeof (GoodreadsResponse));
      var response = (GoodreadsResponse) xmlSerializer.Deserialize (responseStream);

      return response.Search.Works.Select (w => {
        return new Book {
        Id = w.BestBook.Id.ToString (),
        AverageRating = w.AverageRating,
        Title = w.BestBook.Title,
        Authors = new Author[] {
          new Author { Id = w.BestBook.Author.Id.ToString(), Name = w.BestBook.Author.Name }
        },
        ImageUrl = w.BestBook.ImageUrl,
        SmallImageUrl = w.BestBook.SmallImageUrl
        };
      }).ToArray ();

    }
  }
}