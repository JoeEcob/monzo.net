namespace Monzo.Tests.MonzoClientTests;

using Fakes;
using NUnit.Framework;
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

            Assert.AreEqual(true, response.Authenticated);
            Assert.AreEqual("oauth_abc123", response.ClientId);
            Assert.AreEqual("user_00009238aMBIIrS5Rdncq9", response.UserId);
        }

        Assert.AreEqual("/ping/whoami", fakeMessageHandler.Request.RequestUri!.PathAndQuery);

        Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization!.ToString());
    }
}
