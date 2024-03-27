namespace Monzo.Tests.MonzoAuthorizationClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using System.Net;
    using System.Threading.Tasks;

    [TestFixture]
    public sealed class MonzoAuthorizationClientTests
    {
        [Test]
        public void GetLoginPageUrl()
        {
            using (var client = new Monzo.MonzoAuthorizationClient("testClientId", "testClientSecret", "http://foo"))
            {
                var loginPageUrl = client.GetAuthorizeUrl("testState", "testRedirectUri");

                ClassicAssert.AreEqual("https://auth.monzo.com/?response_type=code&client_id=testClientId&state=testState&redirect_uri=testRedirectUri", loginPageUrl);
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

                ClassicAssert.AreEqual("testAccessToken", accessToken.Value);
                ClassicAssert.AreEqual("testRefreshToken", accessToken.RefreshToken);
                ClassicAssert.AreEqual("testUserId", accessToken.UserId);
                ClassicAssert.AreEqual(21600, accessToken.ExpiresIn);
            }

            ClassicAssert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("authorization_code", formCollection["grant_type"]);
            ClassicAssert.AreEqual("testClientId", formCollection["client_id"]);
            ClassicAssert.AreEqual("testClientSecret", formCollection["client_secret"]);
            ClassicAssert.AreEqual("testRedirectUri", formCollection["redirect_uri"]);
            ClassicAssert.AreEqual("testCode", formCollection["code"]);
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

                ClassicAssert.AreEqual("testAccessToken", accessToken.Value);
                ClassicAssert.AreEqual("testRefreshToken", accessToken.RefreshToken);
                ClassicAssert.AreEqual("testUserId", accessToken.UserId);
                ClassicAssert.AreEqual(21600, accessToken.ExpiresIn);
            }

            ClassicAssert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("password", formCollection["grant_type"]);
            ClassicAssert.AreEqual("testClientId", formCollection["client_id"]);
            ClassicAssert.AreEqual("testClientSecret", formCollection["client_secret"]);
            ClassicAssert.AreEqual("testUsername", formCollection["username"]);
            ClassicAssert.AreEqual("testPassword", formCollection["password"]);
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

                ClassicAssert.AreEqual("testAccessToken2", accessToken.Value);
                ClassicAssert.AreEqual("testRefreshToken2", accessToken.RefreshToken);
                ClassicAssert.AreEqual(21600, accessToken.ExpiresIn);
            }


            ClassicAssert.AreEqual("/oauth2/token", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("refresh_token", formCollection["grant_type"]);
            ClassicAssert.AreEqual("testClientId", formCollection["client_id"]);
            ClassicAssert.AreEqual("testClientSecret", formCollection["client_secret"]);
            ClassicAssert.AreEqual("testAccessToken1", formCollection["refresh_token"]);
        }
    }
}
