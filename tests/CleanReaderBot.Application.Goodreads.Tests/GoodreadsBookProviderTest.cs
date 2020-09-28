using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using JustEat.HttpClientInterception;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;
using System.Net.Http;
using AutoMapper;

namespace CleanReaderBot.Application.Goodreads.Tests
{
  public class GoodreadsBookProviderTest
  {
    private readonly IServiceProvider provider;
    private readonly HttpClientInterceptorOptions interceptor;

    public GoodreadsBookProviderTest(IServiceProvider provider, HttpClientInterceptorOptions interceptor) {
      this.provider = provider;
      this.interceptor = interceptor;
    }

    private Stream OpenFile(string path)
    {
      return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    [Fact]
    public void GoodreadsBookProvider__Constructor__Should_Throw_Argument_Exception_When_Key_Is_Not_Provided()
    {
      var client = this.provider.GetService<HttpClient>();
      var mapper = this.provider.GetService<IMapper>();
      var settingsWithNoKey = new GoodreadsAPISettings();

      this.Invoking((_) => new GoodreadsBookProvider(client, Options.Create(settingsWithNoKey), mapper))
          .Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task GoodreadsBookProvider__Search__Returns_A_List_Of_Books() {
      using(this.interceptor.BeginScope()) {
        var GoodreadsAPISettings = this.provider.GetService<IOptions<GoodreadsAPISettings>>();
        var searchBookQuery = new SearchBooks("Ender's Game");

        var builder = new HttpRequestInterceptionBuilder()
          .Requests()
          .ForUri(GoodreadsUriBuilder.BuildFor(searchBookQuery, GoodreadsAPISettings.Value))
          .Responds()
          .WithContentStream(() => Task.FromResult(OpenFile("Fixtures/EndersGame_Response.xml")))
          .RegisterWith(this.interceptor);

        var searchBooksHandler = this.provider.GetService<IHandler<SearchBooks, SearchBooksResult>>();
        
        var result = await searchBooksHandler.Execute(searchBookQuery);
        
        result.Items.Should().HaveCount(20);
      }
    }
  }
}