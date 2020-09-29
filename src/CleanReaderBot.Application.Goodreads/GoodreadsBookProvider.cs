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
using CleanReaderBot.Application.SearchForBooks;


namespace CleanReaderBot.Application.Goodreads
{
  public class GoodreadsBookProvider : IBookProvider
  {
    private readonly HttpClient client;
    private readonly GoodreadsAPISettings settings;
    private readonly IMapper mapper;

    public GoodreadsBookProvider(HttpClient client, IOptions<GoodreadsAPISettings> settings, IMapper mapper) 
    {
        if (String.IsNullOrEmpty(settings.Value.Key)) {
          throw new ArgumentException("The Goodreads API Key is required.");
        }
        
        this.client = client;
        this.settings = settings.Value;
        this.mapper = mapper;
    }

    public async Task<Book[]> Search(SearchBooks query)
    {
      var uri = GoodreadsUriBuilder.BuildFor(query, settings);
      var responseStream = await this.client.GetStreamAsync(uri.ToString());
      var xmlSerializer = new XmlSerializer(typeof(GoodreadsResponse));
      var response = (GoodreadsResponse) xmlSerializer.Deserialize(responseStream);
      
      return response.Result.Works.Select(w => this.mapper.Map<Book>(w.BestBook)).ToArray();
    }
  }
}