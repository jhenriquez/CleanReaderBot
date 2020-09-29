using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using CleanReaderBot.Webhooks.Services;
using CleanReaderBot.Webhooks.IntegrationTests.Services;

namespace CleanReaderBot.Webhooks.IntegrationTests
{
    public class ApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var telegramDescriptors = services.Where(d => d.ImplementationType == typeof(TelegramBotService)).ToList();
                
                foreach(ServiceDescriptor telegramSvcDescriptor in telegramDescriptors) {
                    services.Remove(telegramSvcDescriptor);
                }

                services.AddSingleton<TelegramBotService, TestTelegramBotService>();
            });
        }
    }
}