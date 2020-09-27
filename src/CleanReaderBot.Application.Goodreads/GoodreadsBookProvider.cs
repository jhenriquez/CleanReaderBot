using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using AutoMapper;
using CleanReaderBot.Application.Common.Entities;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;


namespace CleanReaderBot.Application.Goodreads
{
  public class GoodreadsBookProvider : IBookProvider
  {
    private readonly HttpClient client;
    private readonly IOptions<GoodreadsAPISettings> settings;
    private readonly IMapper mapper;

    public GoodreadsBookProvider(HttpClient client, IOptions<GoodreadsAPISettings> settings, IMapper mapper) 
    {
      this.client = client;
      this.settings = settings;
      this.mapper = mapper;
    }

    private UriBuilder BuildUri(SearchBooks query) 
    {
      var uri = new UriBuilder("https://www.goodreads.com/search/index.xml");
      var queryParameters = HttpUtility.ParseQueryString(string.Empty);
      queryParameters["key"] = settings.Value.Key;
      queryParameters["q"] = query.Query;
      queryParameters["field"] = query.Field.ToString();
      uri.Query = queryParameters.ToString();
      return uri;
    }

    public async Task<Book[]> Search(SearchBooks query)
    {
      var uri = this.BuildUri(query);
      var responseStream = await this.client.GetStreamAsync(uri.ToString());
      var xmlSerializer = new XmlSerializer(typeof(GoodreadsResponse));
      var response = (GoodreadsResponse) xmlSerializer.Deserialize(responseStream);
      
      return response.Result.Works.Select(w => this.mapper.Map<Book>(w.BestBook)).ToArray();
    }
  }
}