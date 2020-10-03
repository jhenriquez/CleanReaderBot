using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchForBooks;


namespace CleanReaderBot.Application.BookProvider.Goodreads
{
  public class GoodreadsBookProvider : IBookProvider
  {
    private readonly HttpClient client;
    private readonly GoodreadsAPISettings settings;

    public GoodreadsBookProvider(HttpClient client, IOptions<GoodreadsAPISettings> settings)
    {
        if (String.IsNullOrEmpty(settings.Value.Key)) {
          throw new ArgumentException("The Goodreads API Key is required.");
        }
        
        this.client = client;
        this.settings = settings.Value;
    }

    public async Task<Book[]> Search(SearchBooks query)
    {
      var uri = GoodreadsUriBuilder.BuildFor(query, settings);
      var responseStream = await this.client.GetStreamAsync(uri.ToString());
      var xmlSerializer = new XmlSerializer(typeof(GoodreadsResponse));
      var response = (GoodreadsResponse) xmlSerializer.Deserialize(responseStream);
      
      return response.Result.Works.Select(w => 
      {
        return new Book {
          Id = w.BestBook.Id,
          AverageRating = w.AverageRating,
          Title = w.BestBook.Title,
          Author = new Author {
            Id = w.BestBook.Author.Id,
            Name = w.BestBook.Author.Name
          },
          ImageUrl = w.BestBook.ImageUrl,
          SmallImageUrl = w.BestBook.SmallImageUrl
        };
      }).ToArray();

    }
  }
}