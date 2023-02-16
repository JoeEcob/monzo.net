namespace Monzo.Tests.MonzoAuthorizationClientTests;

using Fakes;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public sealed class MonzoAuthorizationClientTests
{
    [Test]
    public void GetLoginPageUrl()
    {
        using (var client = new MonzoAuthorizationClient("testClientId", "testClientSecret", "http://foo"))
        {
            var loginPageUrl = client.GetAuthorizeUrl("testState", "testRedirectUri");

            Assert.AreEqual("https://auth.monzo.com/?response_type=code&client_id=testClientId&state=testState&redirect_uri=testRedirectUri", loginPageUrl);
        }
    }

    [Test]
    public async Task ExchangeCodeForAccessTokenAsync()
    {
        var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
            @"{
                    'access_token': 'testAccessToken',
                    'client_id': 'client_id',
                    'expires_in': 21600,
                    'refresh_token': 'testRefreshToken',
                    'token_type': 'Bearer',
                    'user_id': 'testUserId'
                }");

        using (var client = new MonzoAuthorizationClient(httpClient, "testClientId", "testClientSecret"))
        {
            var accessToken = await client.GetAccessTokenAsync("testCode", "testRedirectUri");

            Assert.AreEqual("testAccessToken", accessToken.Value);
            Assert.AreEqual("testRefreshToken", accessToken.RefreshToken);
            Assert.AreEqual("testUserId", accessToken.UserId);
            Assert.AreEqual(21600, accessToken.ExpiresIn);
        }

        Assert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri!.PathAndQuery);

        var formCollection = await fakeMessageHandler.GetQueryStringAsync();

        Assert.AreEqual("authorization_code", formCollection["grant_type"]);
        Assert.AreEqual("testClientId", formCollection["client_id"]);
        Assert.AreEqual("testClientSecret", formCollection["client_secret"]);
        Assert.AreEqual("testRedirectUri", formCollection["redirect_uri"]);
        Assert.AreEqual("testCode", formCollection["code"]);
    }

    [Test]
    public async Task Authenticate()
    {
        var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
            @"{
                    'access_token': 'testAccessToken',
                    'client_id': 'client_id',
                    'expires_in': 21600,
                    'refresh_token': 'testRefreshToken',
                    'token_type': 'Bearer',
                    'user_id': 'testUserId'
                }");


        using (var client = new MonzoAuthorizationClient(httpClient, "testClientId", "testClientSecret"))
        {
            var accessToken = await client.AuthenticateAsync("testUsername", "testPassword");

            Assert.AreEqual("testAccessToken", accessToken.Value);
            Assert.AreEqual("testRefreshToken", accessToken.RefreshToken);
            Assert.AreEqual("testUserId", accessToken.UserId);
            Assert.AreEqual(21600, accessToken.ExpiresIn);
        }

        Assert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri!.PathAndQuery);

        var formCollection = await fakeMessageHandler.GetQueryStringAsync();

        Assert.AreEqual("password", formCollection["grant_type"]);
        Assert.AreEqual("testClientId", formCollection["client_id"]);
        Assert.AreEqual("testClientSecret", formCollection["client_secret"]);
        Assert.AreEqual("testUsername", formCollection["username"]);
        Assert.AreEqual("testPassword", formCollection["password"]);
    }

    [Test]
    public async Task RefreshAccessToken()
    {
        var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
            @"{
                    'access_token': 'testAccessToken2',
                    'client_id': 'client_id',
                    'expires_in': 21600,
                    'refresh_token': 'testRefreshToken2',
                    'token_type': 'Bearer',
                    'user_id': 'testUserId'
                }");

        using (var client = new MonzoAuthorizationClient(httpClient, "testClientId", "testClientSecret"))
        {
            var accessToken = await client.RefreshAccessTokenAsync("testAccessToken1");

            Assert.AreEqual("testAccessToken2", accessToken.Value);
            Assert.AreEqual("testRefreshToken2", accessToken.RefreshToken);
            Assert.AreEqual(21600, accessToken.ExpiresIn);
        }


        Assert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri!.PathAndQuery);

        var formCollection = await fakeMessageHandler.GetQueryStringAsync();

        Assert.AreEqual("refresh_token", formCollection["grant_type"]);
        Assert.AreEqual("testClientId", formCollection["client_id"]);
        Assert.AreEqual("testClientSecret", formCollection["client_secret"]);
        Assert.AreEqual("testAccessToken1", formCollection["refresh_token"]);
    }
}
