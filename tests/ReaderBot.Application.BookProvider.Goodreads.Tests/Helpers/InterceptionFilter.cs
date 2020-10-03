using System;
using Microsoft.Extensions.Http;
using JustEat.HttpClientInterception;

namespace ReaderBot.Application.Tests.Helpers
{
  public class InterceptionFilter : IHttpMessageHandlerBuilderFilter
  {
    private readonly HttpClientInterceptorOptions _options;

    internal InterceptionFilter(HttpClientInterceptorOptions options)
    {
      _options = options;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
      return (builder) =>
      {
          // Run any actions the application has configured for itself
          next(builder);

          // Add the interceptor as the last message handler
          builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
      };
    }
  }
}