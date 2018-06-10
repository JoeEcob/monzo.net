using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Webhooks
    {
        [Test]
        public async void CanCreateWebhook()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);
                    Assert.AreEqual("POST", context.Request.Method);
                    Assert.AreEqual("/webhooks", context.Request.Uri.PathAndQuery);

                    var formCollection = await context.Request.ReadFormAsync();
                    Assert.AreEqual("1", formCollection["account_id"]);
                    Assert.AreEqual("http://example.com", formCollection["url"]);

                    await context.Response.WriteAsync(
                        @"{
                            'webhook': {
                                'account_id': 'account_id',
                                'id': 'webhook_id',
                                'url': 'http://example.com'
                            }
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var webhook = await client.CreateWebhookAsync("1", "http://example.com");

                    Assert.AreEqual("account_id", webhook.AccountId);
                    Assert.AreEqual("webhook_id", webhook.Id);
                    Assert.AreEqual("http://example.com", webhook.Url);
                }
            }
        }

        [Test]
        public async void CanGetWebhooks()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/webhooks?account_id=1", context.Request.Uri.PathAndQuery);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync(
                        @"{
                            'webhooks': [
                                {
                                    'account_id': 'acc_000091yf79yMwNaZHhHGzp',
                                    'id': 'webhook_000091yhhOmrXQaVZ1Irsv',
                                    'url': 'http://example.com/callback'
                                }
                            ]
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var webhooks = await client.GetWebhooksAsync("1");

                    Assert.AreEqual(1, webhooks.Count);
                    Assert.AreEqual("webhook_000091yhhOmrXQaVZ1Irsv", webhooks[0].Id);
                    Assert.AreEqual("acc_000091yf79yMwNaZHhHGzp", webhooks[0].AccountId);
                    Assert.AreEqual("http://example.com/callback", webhooks[0].Url);
                }
            }
        }

        [Test]
        public async void CanDeleteWebhook()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/webhooks/1", context.Request.Uri.PathAndQuery);
                    Assert.AreEqual("DELETE", context.Request.Method);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync("{}");
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    await client.DeleteWebhookAsync("1");
                }
            }
        }
    }
}
