using System.Threading.Tasks;
using ReaderBot.Application.GetBookInformation;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Webhooks.Services
{
    public interface IBotService
    {
        Task StartWebhook();
        Task SendSearchResults(SearchBooksResult result, string refMessageId = default);
        Task SendGetBookResult(GetBookResult expectedGetBookResult, string refMessageId = default);
    }
}