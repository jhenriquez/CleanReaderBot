using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;
using Microsoft.Extensions.Options;

namespace CleanReaderBot.Application.Goodreads
{
  public class GoodreadsBookProvider : IBookProvider
  {
    private readonly HttpClient client;
    private readonly IOptions<GoodreadsAPISettings> settings;

    public GoodreadsBookProvider(HttpClient client, IOptions<GoodreadsAPISettings> settings) 
    {
      this.client = client;
      this.settings = settings;
    }

    public async Task<Book[]> Search(SearchBooks query)
    {
      var uri = new UriBuilder("https://www.goodreads.com/search/index.xml");
      var queryParameters = HttpUtility.ParseQueryString(string.Empty);
      queryParameters["key"] = settings.Value.Key;
      queryParameters["q"] = query.Query;
      queryParameters["field"] = query.Field.ToString();
      uri.Query = queryParameters.ToString();
      var responseStream = await this.client.GetStreamAsync(uri.ToString());
      var xmlSerializer = new XmlSerializer(typeof(GoodreadsResponse));
      var response = (GoodreadsResponse) xmlSerializer.Deserialize(responseStream);
      return response.Result.Works.Select(w => new Book { Id = w.BestBook.Id, Title = w.BestBook.Title }).ToArray();
    }
  }
}