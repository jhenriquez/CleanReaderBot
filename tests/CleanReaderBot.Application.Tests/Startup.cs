using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanReaderBot.Application.Tests
{
    public class Startup
    {
        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}