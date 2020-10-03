using ReaderBot.Application.Common.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace ReaderBot.Application.BookProvider.Goodreads
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddReaderBotGoodreadsIntegration(this IServiceCollection services)
    {
      services.AddHttpClient<IBookProvider, GoodreadsBookProvider>();
      return services;
    }
  }
}