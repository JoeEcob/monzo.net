using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Balance
    {
        [Test]
        public async void CanGetBalance()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/balance?account_id=1", context.Request.Uri.PathAndQuery);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync(
                        @"{
                            'balance': 5000,
                            'currency': 'GBP',
                            'spend_today': -100
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var balance = await client.GetBalanceAsync("1");

                    Assert.AreEqual(5000, balance.Value);
                    Assert.AreEqual("GBP", balance.Currency);
                    Assert.AreEqual(-100, balance.SpendToday);
                }
            }
        }
    }
}
