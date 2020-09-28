using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using CleanReaderBot.Application;
using CleanReaderBot.Application.Goodreads;
using CleanReaderBot.Webhooks.Services;

namespace CleanReaderBot.Webhooks
{
  public class Startup
  {

    private IConfiguration Configuration;

    public Startup()
    {
      Configuration = new ConfigurationBuilder()
          .AddUserSecrets<Startup>()
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
        .AddCleanReaderBot()
        .AddCleanReaderBotGoodreadsIntegration()
        .AddCleanReaderBotWebhooksTelegramIntegration()
        .Configure<GoodreadsAPISettings>(Configuration.GetSection("Goodreads"))
        .Configure<TelegramSettings>(Configuration.GetSection("Telegram"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app, IWebHostEnvironment env,
      ISpecificBotService<TelegramBotClient, TelegramSettings> telegram)
    { 
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseCors();


      app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

      telegram.StartWebHook();
    }
  }
}