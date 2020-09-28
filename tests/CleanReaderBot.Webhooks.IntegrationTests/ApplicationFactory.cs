using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using CleanReaderBot.Webhooks.Services;

namespace CleanReaderBot.Webhooks.IntegrationTests
{
    public class ApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                
            });
        }
    }
}