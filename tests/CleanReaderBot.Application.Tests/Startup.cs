using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JustEat.HttpClientInterception;
using CleanReaderBot.Application.Goodreads;
using CleanReaderBot.Application.Tests.Helpers;


namespace CleanReaderBot.Application.Tests
{
    public class Startup
    {
        private IConfiguration Configuration;
        
        public Startup() {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCleanReaderBot();
            services.AddCleanReaderBotGoodreadsIntegration();
            services.Configure<GoodreadsAPISettings>(Configuration.GetSection("Goodreads"));
            var options = new HttpClientInterceptorOptions {ThrowOnMissingRegistration = true};
            services.AddSingleton<HttpClientInterceptorOptions>((_) => options);
            services.AddSingleton<IHttpMessageHandlerBuilderFilter, InterceptionFilter>((_) => new InterceptionFilter(options));
        }
    }
}