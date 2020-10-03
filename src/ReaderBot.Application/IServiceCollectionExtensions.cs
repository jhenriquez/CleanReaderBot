using Microsoft.Extensions.DependencyInjection;
using ReaderBot.Application.Common.Interfaces;

namespace ReaderBot.Application
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddReaderBot(this IServiceCollection services)
    {
      services.Scan(scan => scan
          .FromCallingAssembly()
          .AddClasses(c => c.AssignableTo(typeof(IHandler<,>)))
            .AsImplementedInterfaces());

      return services;
    }
  }
}