using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Xunit;

namespace CleanReaderBot.Webhooks.IntegrationTests.Messages {
    public class Telegram : IClassFixture<ApplicationFactory<Startup>> {
        private HttpClient Client { get; }

        private Update SampleUpdateMessage { get; }

        public Telegram (ApplicationFactory<Startup> factory) {
            Client = factory.CreateClient ();
        }

        [Fact]
        public async Task Post__Returns_Ok () {
            var rand = new Random ();

            var updateMessage = new Update {
                Id = rand.Next (1000, 1500),
                InlineQuery = new InlineQuery {
                    Id = rand.Next (1000, 1500).ToString (),
                    From = new User {
                        Id = rand.Next (1000, 1500),
                        IsBot = false,
                        FirstName = rand.Next (1000, 1500).ToString (),
                        LastName = rand.Next (1000, 1500).ToString (),
                        Username = rand.Next (1000, 1500).ToString ()
                    },
                Query = "Sample Query",
                Offset = ""
                }
            };

            var messagePayload = JsonConvert.SerializeObject (updateMessage);
            var response = await Client.PostAsync ("/messages/telegram", new StringContent (messagePayload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode ();
        }
    }
}