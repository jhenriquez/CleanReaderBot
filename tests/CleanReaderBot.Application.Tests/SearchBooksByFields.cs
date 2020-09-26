using Xunit;
using FluentAssertions;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;
using System.Threading.Tasks;

namespace CleanReaderBot.Application.Tests
{
  public class SearchBooksByFields
  {
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
  }
}