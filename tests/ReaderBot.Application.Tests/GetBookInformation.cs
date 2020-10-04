using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReaderBot.Application.Common.Interfaces;
using ReaderBot.Application.GetBookInformation;
using Xunit;

namespace ReaderBot.Application.Tests {
    public class GetBookInformation {
        [Fact]
        public void GetBook__GetBook__Throws_ArgumentException_When_Id_Is_Null_Or_Empty () {
            this.Invoking ((_) => new GetBook ("")).Should ().Throw<ArgumentException> ();
        }

        [Fact]
        public void GetBook__GetBook__Assigns_Id_to_BookId_Property () {
            var getBookQuery = new GetBook ("Id");
            getBookQuery.BookId.Should ().Be ("Id");
        }

        [Fact]
        public void GetBook__Handler__Throws_ArgumentNullException_When_BookProvider_Is_Null()
        {
            this.Invoking((_) => new GetBook.Handler(null)).Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public async Task GetBook__Handler__Uses_BookProvider__GetBook__Method () {
            var bookProvider = Substitute.For<IBookProvider>();
            var getBookQuery = new GetBook("Id");
            var getBookHandler = new GetBook.Handler(bookProvider);
            await getBookHandler.Execute(getBookQuery);
            await bookProvider.Received().GetBook(getBookQuery);
        }
    }
}