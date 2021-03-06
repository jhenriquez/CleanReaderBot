using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.SearchForBooks;
using Xunit;

namespace ReaderBot.Application.Tests {
  public class SearchForBooks {
    private Stream OpenFile (string path) {
      return File.Open (path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    [Fact]
    public void SearchBooks__Query__Is_Empty_String_When_Null_Is_Provided () {
      var searchBooks = new SearchBooks (null);
      searchBooks.Query.Should ().BeEmpty ();
    }

    [Fact]
    public void SearchBooks__Query__Should_Default_To_Empty_String () {
      var searchBooks = new SearchBooks ();
      searchBooks.Query.Should ().BeEmpty ();
    }

    [Fact]
    public void SearchBooks__Field__Should_Default_To_All () {
      var searchBookQuery = new SearchBooks ();
      searchBookQuery.Field.Should ().Equals (SearchableFields.All);
    }

    [Fact]
    public async Task SearchBooks__Handler__Uses_BookProvider__Search_Method () {
      var bookProvider = Substitute.For<IBookProvider> ();
      var searchBooksQuery = new SearchBooks ("Ender's Game");
      var searchBooksHandler = new SearchBooks.Handler (bookProvider);
      await searchBooksHandler.Execute (searchBooksQuery);
      await bookProvider.Received ().Search (searchBooksQuery);
    }
  }
}