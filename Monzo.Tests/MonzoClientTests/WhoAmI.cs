namespace Monzo.Tests.MonzoClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using System.Net;
    using System.Threading.Tasks;

    [TestFixture]
    public sealed class WhoAmI
    {
        [Test]
        public async Task CanGetWhoAmI()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'authenticated': true,
                    'client_id': 'oauth_abc123',
                    'user_id': 'user_00009238aMBIIrS5Rdncq9',
                }");


            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var response = await client.WhoAmIAsync();

                ClassicAssert.AreEqual(true, response.Authenticated);
                ClassicAssert.AreEqual("oauth_abc123", response.ClientId);
                ClassicAssert.AreEqual("user_00009238aMBIIrS5Rdncq9", response.UserId);
            }

            ClassicAssert.AreEqual("/ping/whoami", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
