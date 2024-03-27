namespace Monzo.Tests.MonzoClientTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

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

                                ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            ClassicAssert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());
            ClassicAssert.AreEqual("/feed", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();
            ClassicAssert.AreEqual("1", formCollection["account_id"]);
            ClassicAssert.AreEqual("basic", formCollection["type"]);
            ClassicAssert.AreEqual("https://www.example.com/a_page_to_open_on_tap.html", formCollection["url"]);
            ClassicAssert.AreEqual("My custom item", formCollection["params[title]"]);
        }
    }
}
