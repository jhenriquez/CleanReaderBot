using System.Threading.Tasks;
using ReaderBot.Application.Common.Entities;
using ReaderBot.Application.SearchForBooks;

namespace ReaderBot.Webhooks.Services
{
    public interface IBotService
    {
        Task StartWebhook();

        Task SendSearchResults(SearchBooksResult result, string messageId = default);
    }
}