namespace Monzo.Tests.MonzoClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using System.Net;
    using System.Threading.Tasks;

    [TestFixture]
    public sealed class Balance
    {
        [Test]
        public async Task CanGetBalance()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'balance': 5000,
                    'currency': 'GBP',
                    'spend_today': -100
                }");
 
            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var balance = await client.GetBalanceAsync("1");

                Assert.AreEqual(5000, balance.Value);
                Assert.AreEqual("GBP", balance.Currency);
                Assert.AreEqual(-100, balance.SpendToday);
            }

            Assert.AreEqual("/balance?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
