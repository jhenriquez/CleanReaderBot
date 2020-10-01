using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using JustEat.HttpClientInterception;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchForBooks;
using System.Net.Http;
using AutoMapper;
using System.Linq;

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

    [Fact]
    public async Task GoodreadsBookProvider__Search__Parses_Xml_Correctly__Validate_First_And_Last_Element_Content() {
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
        
        var firstElement = result.Items.First();
        var lastElement = result.Items.Last();

        firstElement.Should().NotBeNull();
        firstElement.Id.Should().Be(375802);
        firstElement.Title.Should().Be("Ender's Game (Ender's Saga, #1)");
        firstElement.Author.Id.Should().Be(589);
        firstElement.Author.Name.Should().Be("Orson Scott Card");
        firstElement.AverageRating.Should().BeGreaterThan(4);
        firstElement.ImageUrl.Should().Be("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY160_.jpg");
        firstElement.SmallImageUrl.Should().Be("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY75_.jpg");

        lastElement.Should().NotBeNull();
        lastElement.Id.Should().Be(33144572);
        lastElement.Title.Should().Be("End Game (Fallen Empire, #8)");
        lastElement.Author.Id.Should().Be(4512224);
        lastElement.Author.Name.Should().Be("Lindsay Buroker");
        lastElement.AverageRating.Should().BeGreaterThan(4);
        lastElement.ImageUrl.Should().Be("https://s.gr-assets.com/assets/nophoto/book/111x148-bcc042a9c91a29c1d680899eff700a03.png");
        lastElement.SmallImageUrl.Should().Be("https://s.gr-assets.com/assets/nophoto/book/50x75-a91bf249278a81aabab721ef782c4a74.png");
      }
    }
  }
}