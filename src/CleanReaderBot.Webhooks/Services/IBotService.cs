using System.Threading.Tasks;
using CleanReaderBot.Application.SearchForBooks;

namespace CleanReaderBot.Webhooks.Services
{
    public interface IBotService
    {
        Task StartWebhook();

        Task SendSearchResults(SearchBooksResult result, string messageId = default);
    }
}