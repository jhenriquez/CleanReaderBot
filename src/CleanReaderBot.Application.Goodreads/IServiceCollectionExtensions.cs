using CleanReaderBot.Application.Common.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace CleanReaderBot.Application.Goodreads
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBotGoodreadsIntegration(this IServiceCollection services)
    {
      services.AddHttpClient<IBookProvider, GoodreadsBookProvider>();
      return services;
    }
  }
}