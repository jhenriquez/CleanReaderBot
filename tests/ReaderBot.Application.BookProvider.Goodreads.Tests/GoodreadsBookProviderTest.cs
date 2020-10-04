using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;
using Xunit;

namespace ReaderBot.Application.BookProvider.Goodreads.Tests {
  public class GoodreadsBookProviderTest {
    private IServiceProvider Provider { get; }
    private HttpClientInterceptorOptions Interceptor { get; }

    public GoodreadsBookProviderTest (IServiceProvider provider, HttpClientInterceptorOptions interceptor) {
      Provider = provider;
      Interceptor = interceptor;
    }

    private Stream OpenFile (string path) {
      return File.Open (path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    [Fact]
    public void Constructor__Should_Throw_Argument_Exception_When_Key_Is_Not_Provided () {
      var client = Provider.GetService<HttpClient> ();
      var settingsWithNoKey = new GoodreadsAPISettings ();

      this.Invoking ((_) => new GoodreadsBookProvider (client, Options.Create (settingsWithNoKey)))
        .Should ().Throw<ArgumentException> ();
    }

    [Fact]
    public async Task Search__Returns_A_List_Of_Books () {
      using (Interceptor.BeginScope ()) {
        var GoodreadsAPISettings = Provider.GetService<IOptions<GoodreadsAPISettings>> ();
        var searchBookQuery = new SearchBooks ("Ender's Game");

        new HttpRequestInterceptionBuilder ()
          .Requests ()
          .ForUri (GoodreadsUriBuilder.BuildFor (searchBookQuery, GoodreadsAPISettings.Value))
          .Responds ()
          .WithContentStream (() => Task.FromResult (OpenFile ("Fixtures/EndersGame_BookSearch_Response.xml")))
          .RegisterWith (Interceptor);

        var searchBooksHandler = Provider.GetService<IHandler<SearchBooks, SearchBooksResult>> ();

        var result = await searchBooksHandler.Execute (searchBookQuery);

        result.Items.Should ().HaveCount (20);
      }
    }

    [Fact]
    public async Task Search__Parses_Xml_Correctly__Validate_First_And_Last_Element_Content () {
      using (Interceptor.BeginScope ()) {
        var GoodreadsAPISettings = Provider.GetService<IOptions<GoodreadsAPISettings>> ();
        var searchBookQuery = new SearchBooks ("Ender's Game");

        new HttpRequestInterceptionBuilder ()
          .Requests ()
          .ForUri (GoodreadsUriBuilder.BuildFor (searchBookQuery, GoodreadsAPISettings.Value))
          .Responds ()
          .WithContentStream (() => Task.FromResult (OpenFile ("Fixtures/EndersGame_BookSearch_Response.xml")))
          .RegisterWith (Interceptor);

        var searchBooksHandler = Provider.GetService<IHandler<SearchBooks, SearchBooksResult>> ();
        var result = await searchBooksHandler.Execute (searchBookQuery);
        var firstElement = result.Items.First ();
        var lastElement = result.Items.Last ();

        firstElement.Should ().NotBeNull ();
        firstElement.Id.Should ().Be ("375802");
        firstElement.Title.Should ().Be ("Ender's Game (Ender's Saga, #1)");
        firstElement.Authors.Should ().Equal (new Author[] { new Author { Id = "589", Name = "Orson Scott Card" } });
        firstElement.AverageRating.Should ().BeGreaterThan (4);
        firstElement.ImageUrl.Should ().Be ("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY160_.jpg");
        firstElement.SmallImageUrl.Should ().Be ("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY75_.jpg");

        lastElement.Should ().NotBeNull ();
        lastElement.Id.Should ().Be ("33144572");
        lastElement.Title.Should ().Be ("End Game (Fallen Empire, #8)");
        lastElement.Authors.Should ().Equal (new Author[] { new Author { Id = "4512224", Name = "Lindsay Buroker" } });
        lastElement.AverageRating.Should ().BeGreaterThan (4);
        lastElement.ImageUrl.Should ().Be ("https://s.gr-assets.com/assets/nophoto/book/111x148-bcc042a9c91a29c1d680899eff700a03.png");
        lastElement.SmallImageUrl.Should ().Be ("https://s.gr-assets.com/assets/nophoto/book/50x75-a91bf249278a81aabab721ef782c4a74.png");
      }
    }

    [Fact]
    public async Task GetBook__Returns_A_Book_For_A_Matching_Id () {
      using (Interceptor.BeginScope ()) {
        var GoodreadsAPISettings = Provider.GetService<IOptions<GoodreadsAPISettings>> ();
        var getBookQuery = new GetBook ("375802");

        new HttpRequestInterceptionBuilder ()
          .Requests ()
          .ForUri (GoodreadsUriBuilder.BuildFor (getBookQuery, GoodreadsAPISettings.Value))
          .Responds ()
          .WithContentStream (() => Task.FromResult (OpenFile ("Fixtures/375802_BookShow_Response.xml")))
          .RegisterWith (Interceptor);

        var searchBooksHandler = Provider.GetService<IHandler<GetBook, GetBookResult>> ();

        var result = await searchBooksHandler.Execute (getBookQuery);

        var expectedAuthors = new Author[] {
          new Author { Id = "589", Name = "Orson Scott Card" },
          new Author { Id = "44767", Name = "Stefan Rudnicki" },
          new Author { Id = "7415", Name = "Harlan Ellison" }
        };

        result.Book.Should ().NotBeNull ();
        result.Book.Should ().NotBeNull ();
        result.Book.Id.Should ().Be ("375802");
        result.Book.Title.Should ().Be ("Ender's Game (Ender's Saga, #1)");
        result.Book.Authors.Should ().Equal (expectedAuthors);
        result.Book.AverageRating.Should().BeGreaterThan (4);
        result.Book.Description.Should().Be("Andrew \"Ender\" Wiggin thinks he is playing computer simulated war games; he is, in fact, engaged in something far more desperate. The result of genetic experimentation, Ender may be the military genius Earth desperately needs in a war against an alien enemy seeking to destroy all human life. The only way to find out is to throw Ender into ever harsher training, to chip away and find the diamond inside, or destroy him utterly. Ender Wiggin is six years old when it begins. He will grow up fast.<br /><br />But Ender is not the only result of the experiment. The war with the Buggers has been raging for a hundred years, and the quest for the perfect general has been underway almost as long. Ender's two older siblings, Peter and Valentine, are every bit as unusual as he is, but in very different ways. While Peter was too uncontrollably violent, Valentine very nearly lacks the capability for violence altogether. Neither was found suitable for the military's purpose. But they are driven by their jealousy of Ender, and by their inbred drive for power. Peter seeks to control the political process, to become a ruler. Valentine's abilities turn more toward the subtle control of the beliefs of commoner and elite alike, through powerfully convincing essays. Hiding their youth and identities behind the anonymity of the computer networks, these two begin working together to shape the destiny of Earth-an Earth that has no future at all if their brother Ender fails.<br /><br />Source: hatrack.com");
        result.Book.ImageUrl.Should ().Be ("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY160_.jpg");
        result.Book.SmallImageUrl.Should ().Be ("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1408303130l/375802._SY75_.jpg");
      }
    }

    [Fact]
    public async Task GetBook__Book_Value_Is_Null_When_Provided_Book_Id_Is_Invalid () {
      using (Interceptor.BeginScope ()) {
        var getBookQuery = new GetBook ("NonExistentId");
        var GoodreadsAPISettings = Provider.GetService<IOptions<GoodreadsAPISettings>> ();

        new HttpRequestInterceptionBuilder ()
          .Requests ()
          .ForUri (GoodreadsUriBuilder.BuildFor (getBookQuery, GoodreadsAPISettings.Value))
          .Responds ()
          .WithContentStream (() => Task.FromResult (OpenFile ("Fixtures/InvalidId_BookShow_Response.xml")))
          .RegisterWith (Interceptor);

        var searchBooksHandler = Provider.GetService<IHandler<GetBook, GetBookResult>> ();
        var result = await searchBooksHandler.Execute (getBookQuery);

        result.Book.Should().BeNull();
      }
    }

    [Fact]
    public async Task GetBook__Book_Value_Is_Null_When_Provided_Book_Id_Is_Not_Found () {
      using (Interceptor.BeginScope ()) {
        var getBookQuery = new GetBook ("NonExistentId");
        var GoodreadsAPISettings = Provider.GetService<IOptions<GoodreadsAPISettings>> ();

        new HttpRequestInterceptionBuilder ()
          .Requests ()
          .ForUri (GoodreadsUriBuilder.BuildFor (getBookQuery, GoodreadsAPISettings.Value))
          .Responds ()
          .WithContentStream (() => Task.FromResult (OpenFile ("Fixtures/0000000000_BookShow_Response.xml")))
          .RegisterWith (Interceptor);

        var searchBooksHandler = Provider.GetService<IHandler<GetBook, GetBookResult>> ();
        var result = await searchBooksHandler.Execute (getBookQuery);

        result.Book.Should().BeNull();
      }
    }
  }
}