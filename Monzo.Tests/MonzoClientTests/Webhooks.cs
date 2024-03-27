namespace Monzo.Tests.MonzoClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
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

                ClassicAssert.AreEqual("account_id", webhook.AccountId);
                ClassicAssert.AreEqual("webhook_id", webhook.Id);
                ClassicAssert.AreEqual("http://example.com", webhook.Url);
            }

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            ClassicAssert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());
            ClassicAssert.AreEqual("/webhooks", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();
            ClassicAssert.AreEqual("1", formCollection["account_id"]);
            ClassicAssert.AreEqual("http://example.com", formCollection["url"]);
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

                ClassicAssert.AreEqual(1, webhooks.Count);
                ClassicAssert.AreEqual("webhook_000091yhhOmrXQaVZ1Irsv", webhooks[0].Id);
                ClassicAssert.AreEqual("acc_000091yf79yMwNaZHhHGzp", webhooks[0].AccountId);
                ClassicAssert.AreEqual("http://example.com/callback", webhooks[0].Url);
            }

            ClassicAssert.AreEqual("/webhooks?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanDeleteWebhook()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK, "{}");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                await client.DeleteWebhookAsync("1");
            }

            ClassicAssert.AreEqual("/webhooks/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);
            ClassicAssert.AreEqual("DELETE", fakeMessageHandler.Request.Method.ToString());

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
