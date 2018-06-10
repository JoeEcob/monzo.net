using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class WhoAmI
    {
        [Test]
        public async void CanGetWhoAmI()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/ping/whoami", context.Request.Uri.PathAndQuery);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync(
                        @"{
                            'authenticated': true,
                            'client_id': 'oauth_abc123',
                            'user_id': 'user_00009238aMBIIrS5Rdncq9',
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var response = await client.WhoAmIAsync();

                    Assert.AreEqual(true, response.Authenticated);
                    Assert.AreEqual("oauth_abc123", response.ClientId);
                    Assert.AreEqual("user_00009238aMBIIrS5Rdncq9", response.UserId);
                }
            }
        }
    }
}
