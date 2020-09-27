using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using JustEat.HttpClientInterception;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;
using System.IO;

namespace CleanReaderBot.Application.Tests
{
  public class SearchBooksByFields
  {
    private readonly IServiceProvider provider;
    private readonly HttpClientInterceptorOptions interceptor;

    public SearchBooksByFields(IServiceProvider provider, HttpClientInterceptorOptions interceptor) {
      this.provider = provider;
      this.interceptor = interceptor;
    }

    private Stream OpenFile(string path)
    {
      return File.Open(path, FileMode.Open);
    }

    [Fact]
    public void SearchBooks_Query_Is_Empty_String_When_Null_Is_Provided() {
      var searchBooks = new SearchBooks(null);
      searchBooks.Query.Should().BeEmpty();
    }

    [Fact]
    public void SearchBooks_Query_Should_Default_To_Empty_String() {
      var searchBooks = new SearchBooks();
      searchBooks.Query.Should().BeEmpty();
    }

    [Fact]
    public void SearchBooks_Field_Should_Default_To_All() {
      var searchBookQuery = new SearchBooks();
      searchBookQuery.Field.Should().Equals(SearchableFields.All);
    }

    [Fact]
    public async Task SearchBooks_Result_Contains_List_Of_Books() {
      using(this.interceptor.BeginScope()) {
        var builder = new HttpRequestInterceptionBuilder()
          .Requests()
          .For((req) => req.RequestUri.ToString().Contains("goodreads.com/search/index.xml"))
          .Responds()
          .WithContentStream(() => Task.FromResult(OpenFile("Fixtures/EndersGame_Response.xml")))
          .RegisterWith(this.interceptor);

        var searchBooksHandler = this.provider.GetService<IHandler<SearchBooks, SearchBooksResult>>();
        
        var searchBookQuery = new SearchBooks("Ender's Game");
        
        var result = await searchBooksHandler.Execute(searchBookQuery);
        
        result.Items.Should().HaveCount(20);
      }
    }
  }
}