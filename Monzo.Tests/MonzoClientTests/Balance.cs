namespace Monzo.Tests.MonzoClientTests
{
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
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

                ClassicAssert.AreEqual(5000, balance.Value);
                ClassicAssert.AreEqual("GBP", balance.Currency);
                ClassicAssert.AreEqual(-100, balance.SpendToday);
            }

            ClassicAssert.AreEqual("/balance?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
