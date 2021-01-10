namespace Monzo.Tests.MonzoClientTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;

    [TestFixture]
    public sealed class Feed
    {
        [Test]
        public async Task CanCreateFeedItem()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK, "{}");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                await client.CreateFeedItemAsync("1", "basic", new Dictionary<string, string> { { "title", "My custom item" } }, "https://www.example.com/a_page_to_open_on_tap.html");
            }

                                Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            Assert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());
            Assert.AreEqual("/feed", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();
            Assert.AreEqual("1", formCollection["account_id"]);
            Assert.AreEqual("basic", formCollection["type"]);
            Assert.AreEqual("https://www.example.com/a_page_to_open_on_tap.html", formCollection["url"]);
            Assert.AreEqual("My custom item", formCollection["params[title]"]);
        }
    }
}
