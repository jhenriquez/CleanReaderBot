using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Xunit;
using NSubstitute;

namespace CleanReaderBot.Webhooks.Tests.Messages
{
    public class TelegramTests : IClassFixture<ApplicationFactory<Startup>>
    {
        private HttpClient Client { get; }

        private Update SampleUpdateMessage { get; }

        public TelegramTests(ApplicationFactory<Startup> factory) {
            Client = factory.CreateClient();
        }

        private Task<HttpResponseMessage> ExecuteRequest()
        {
            var rand = new Random();

            var updateMessage = new Update {
                Id = rand.Next(1000, 1500),
                InlineQuery = new InlineQuery {
                    Id = rand.Next(1000, 1500).ToString(),
                    From = new User {
                        Id = rand.Next(1000, 1500),
                        IsBot = false,
                        FirstName = rand.Next(1000, 1500).ToString(),
                        LastName = rand.Next(1000, 1500).ToString(),
                        Username = rand.Next(1000, 1500).ToString()
                    },
                    Query = "Sample Query",
                    Offset = ""
                }
            };

            var messagePayload = JsonConvert.SerializeObject(updateMessage);
            return Client.PostAsync("/messages/telegram", new StringContent(messagePayload, Encoding.UTF8, "application/json"));
        }

        [Fact]
        public async Task Post__Returns_Ok()
        {
            var response = await ExecuteRequest();
            response.EnsureSuccessStatusCode();
        }
    }
}