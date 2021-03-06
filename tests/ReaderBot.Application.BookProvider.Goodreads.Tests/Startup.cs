using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JustEat.HttpClientInterception;
using ReaderBot.Application.Tests.Helpers;

namespace ReaderBot.Application.BookProvider.Goodreads.Tests
{
    public class Startup
    {
        private IConfiguration Configuration;
        
        public Startup() {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddReaderBot();
            services.AddReaderBotGoodreadsIntegration();
            services.Configure<GoodreadsAPISettings>(Configuration.GetSection("Goodreads"));
            
            var options = new HttpClientInterceptorOptions {ThrowOnMissingRegistration = true};
            services.AddSingleton<HttpClientInterceptorOptions>((_) => options);
            services.AddSingleton<IHttpMessageHandlerBuilderFilter, InterceptionFilter>((_) => new InterceptionFilter(options));
        }
    }
}