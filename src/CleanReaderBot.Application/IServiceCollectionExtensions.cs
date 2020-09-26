using Microsoft.Extensions.DependencyInjection;
using CleanReaderBot.Application.Common.Interfaces;

namespace CleanReaderBot.Application
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBot(this IServiceCollection services)
    {
      services.Scan(scan => scan
          .FromCallingAssembly()
          .AddClasses(c => c.AssignableTo(typeof(IHandler<,>)))
            .AsImplementedInterfaces());

      return services;
    }
  }
}