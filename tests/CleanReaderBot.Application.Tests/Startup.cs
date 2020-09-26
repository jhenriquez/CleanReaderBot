using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanReaderBot.Application.Goodreads;

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
        }
    }
}