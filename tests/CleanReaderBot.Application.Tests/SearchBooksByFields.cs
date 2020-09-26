using Xunit;
using FluentAssertions;
using CleanReaderBot.Application.Common.Interfaces;
using CleanReaderBot.Application.SearchBooksByFields;
using System.Threading.Tasks;

namespace CleanReaderBot.Application.Tests
{
  public class SearchBooksByFields
  {
    private readonly IHandler<SearchBooks, SearchBooksResult> searchBooksHandler;

    public SearchBooksByFields(IHandler<SearchBooks, SearchBooksResult> searchBooksHandler) {
      this.searchBooksHandler = searchBooksHandler;
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
      var searchBookQuery = new SearchBooks("Ender's Game");
      var result = await searchBooksHandler.Execute(searchBookQuery);
      result.Items.Should().HaveCount(20);
    }
  }
}