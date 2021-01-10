namespace Monzo.Tests.MonzoClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using System.Net;
    using System.Threading.Tasks;

    [TestFixture]
    public sealed class Webhooks
    {
        [Test]
        public async Task CanCreateWebhook()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'webhook': {
                        'account_id': 'account_id',
                        'id': 'webhook_id',
                        'url': 'http://example.com'
                    }
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var webhook = await client.CreateWebhookAsync("1", "http://example.com");

                Assert.AreEqual("account_id", webhook.AccountId);
                Assert.AreEqual("webhook_id", webhook.Id);
                Assert.AreEqual("http://example.com", webhook.Url);
            }

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            Assert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());
            Assert.AreEqual("/webhooks", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();
            Assert.AreEqual("1", formCollection["account_id"]);
            Assert.AreEqual("http://example.com", formCollection["url"]);
        }

        [Test]
        public async Task CanGetWebhooks()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'webhooks': [
                        {
                            'account_id': 'acc_000091yf79yMwNaZHhHGzp',
                            'id': 'webhook_000091yhhOmrXQaVZ1Irsv',
                            'url': 'http://example.com/callback'
                        }
                    ]
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var webhooks = await client.GetWebhooksAsync("1");

                Assert.AreEqual(1, webhooks.Count);
                Assert.AreEqual("webhook_000091yhhOmrXQaVZ1Irsv", webhooks[0].Id);
                Assert.AreEqual("acc_000091yf79yMwNaZHhHGzp", webhooks[0].AccountId);
                Assert.AreEqual("http://example.com/callback", webhooks[0].Url);
            }

            Assert.AreEqual("/webhooks?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanDeleteWebhook()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK, "{}");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                await client.DeleteWebhookAsync("1");
            }

            Assert.AreEqual("/webhooks/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);
            Assert.AreEqual("DELETE", fakeMessageHandler.Request.Method.ToString());

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
