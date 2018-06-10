using System.Collections.Generic;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Feed
    {
        [Test]
        public async void CanCreateFeedItem()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);
                    Assert.AreEqual("POST", context.Request.Method);
                    Assert.AreEqual("/feed", context.Request.Uri.PathAndQuery);

                    var formCollection = await context.Request.ReadFormAsync();
                    Assert.AreEqual("1", formCollection["account_id"]);
                    Assert.AreEqual("basic", formCollection["type"]);
                    Assert.AreEqual("https://www.example.com/a_page_to_open_on_tap.html", formCollection["url"]);
                    Assert.AreEqual("My custom item", formCollection["params[title]"]);

                    await context.Response.WriteAsync("{}");
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    await client.CreateFeedItemAsync("1", "basic", new Dictionary<string, string> { { "title", "My custom item" } }, "https://www.example.com/a_page_to_open_on_tap.html");
                }
            }
        }
    }
}
