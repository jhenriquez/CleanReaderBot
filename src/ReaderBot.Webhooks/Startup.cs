using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ReaderBot.Application;
using ReaderBot.Application.BookProvider.Goodreads;
using ReaderBot.Webhooks.Services;
using ReaderBot.Webhooks.Models;

namespace ReaderBot.Webhooks
{
  public class Startup
  {
    private IConfiguration Configuration;

    public Startup()
    {
      Configuration = new ConfigurationBuilder()
          .AddEnvironmentVariables()
          .Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddControllers()
        .AddNewtonsoftJson();

      services
        .AddReaderBot()
        .AddReaderBotGoodreadsIntegration()
        .AddReaderBotWebhooksIntegration()
        .Configure<GoodreadsAPISettings>(Configuration.GetSection("Goodreads"))
        .Configure<TelegramSettings>(Configuration.GetSection("Telegram"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEnumerable<IBotService> bots)
    { 
      app.UseRouting();
      app.UseCors();

      app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

      foreach (IBotService bot in bots) {
        bot.StartWebhook();
      }
    }
  }
}