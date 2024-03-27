namespace Monzo.Tests.MonzoClientTests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public sealed class Pots
    {
        [Test]
        public async Task CanGetPots()
        {
            var currentAccountId = "this_is_a_current_account";

            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'pots': [
                        {
                            'id': 'pot_0000123456789',
                            'name': 'Rainy Day',
                            'style': 'beach_ball',
                            'balance': 500,
                            'currency': 'GBP',
                            'type': 'default',
                            'minimum_balance': -1,
                            'maximum_balance': -1,
                            'assigned_permissions': [],
                            'current_account_id': 'this_is_a_current_account',
                            'round_up': false,
                            'created': '2017-12-01T23:00:26.256Z',
                            'updated': '2018-01-22T08:12:49.497Z',
                            'deleted': false,
                            'locked': false,
                            'charity_id': ''
                        }
                    ]
                }");


            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var pots = await client.GetPotsAsync(currentAccountId);

                ClassicAssert.AreEqual(1, pots.Count);

                var pot = pots[0];
                ClassicAssert.AreEqual("pot_0000123456789", pot.Id);
                ClassicAssert.AreEqual("Rainy Day", pot.Name);
                ClassicAssert.AreEqual("beach_ball", pot.Style);
                ClassicAssert.AreEqual(500, pot.Balance);
                ClassicAssert.AreEqual("GBP", pot.Currency);
                ClassicAssert.AreEqual("this_is_a_current_account", pot.CurrentAccountId);
                ClassicAssert.AreEqual(new DateTime(2017, 12, 1, 23, 00, 26, 256, DateTimeKind.Utc), pot.Created);
                ClassicAssert.AreEqual(new DateTime(2018, 1, 22, 8, 12, 49, 497, DateTimeKind.Utc), pot.Updated);
                ClassicAssert.AreEqual(false, pot.RoundUp);
                ClassicAssert.AreEqual(false, pot.Locked);
                ClassicAssert.AreEqual(false, pot.Deleted);
            }

            ClassicAssert.AreEqual($"/pots?current_account_id={currentAccountId}", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanDepositIntoPot()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'id': 'pot_00009exampleP0tOxWb',
                    'name': 'Wedding Fund',
                    'style': 'beach_ball',
                    'balance': 550100,
                    'currency': 'GBP',
                    'current_account_id': 'test_account_123',
                    'created': '2017-11-09T12:30:53.695Z',
                    'updated': '2018-02-26T07:12:04.925Z',
                    'deleted': false
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var updatedPot = await client.DepositIntoPotAsync("pot_00009exampleP0tOxWb", "test_account_123", 72932, "a_unique_id");

                ClassicAssert.NotNull(updatedPot);

                ClassicAssert.AreEqual("pot_00009exampleP0tOxWb", updatedPot.Id);
                ClassicAssert.AreEqual("Wedding Fund", updatedPot.Name);
                ClassicAssert.AreEqual("beach_ball", updatedPot.Style);
                ClassicAssert.AreEqual(550100, updatedPot.Balance);
                ClassicAssert.AreEqual("GBP", updatedPot.Currency);
                ClassicAssert.AreEqual("test_account_123", updatedPot.CurrentAccountId);
                ClassicAssert.AreEqual(new DateTime(2017, 11, 9, 12, 30, 53, 695, DateTimeKind.Utc), updatedPot.Created);
                ClassicAssert.AreEqual(new DateTime(2018, 2, 26, 7, 12, 4, 925, DateTimeKind.Utc), updatedPot.Updated);
                ClassicAssert.AreEqual(false, updatedPot.Deleted);
            }
            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            ClassicAssert.AreEqual("PUT", fakeMessageHandler.Request.Method.ToString());
            ClassicAssert.AreEqual("/pots/pot_00009exampleP0tOxWb/deposit", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("test_account_123", formCollection["source_account_id"]);
            ClassicAssert.AreEqual("72932", formCollection["amount"]);
            ClassicAssert.AreEqual("a_unique_id", formCollection["dedupe_id"]);
        }

        [Test]
        public async Task CanWithdrawFromPot()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'id': 'pot_00009exampleP0tOxWb',
                    'name': 'Flying Lessons',
                    'style': 'blue',
                    'balance': 350000,
                    'currency': 'GBP',
                    'current_account_id': 'test_account_123',
                    'created': '2017-11-09T12:30:53.695Z',
                    'updated': '2018-02-26T07:12:04.925Z',
                    'deleted': false
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var updatedPot = await client.WithdrawFromPotAsync("pot_00009exampleP0tOxWb", "test_account_123", 500, "a_unique_id");

                ClassicAssert.NotNull(updatedPot);

                ClassicAssert.AreEqual("pot_00009exampleP0tOxWb", updatedPot.Id);
                ClassicAssert.AreEqual("Flying Lessons", updatedPot.Name);
                ClassicAssert.AreEqual("blue", updatedPot.Style);
                ClassicAssert.AreEqual(350000, updatedPot.Balance);
                ClassicAssert.AreEqual("GBP", updatedPot.Currency);
                ClassicAssert.AreEqual("test_account_123", updatedPot.CurrentAccountId);
                ClassicAssert.AreEqual(new DateTime(2017, 11, 9, 12, 30, 53, 695, DateTimeKind.Utc), updatedPot.Created);
                ClassicAssert.AreEqual(new DateTime(2018, 2, 26, 7, 12, 4, 925, DateTimeKind.Utc), updatedPot.Updated);
                ClassicAssert.AreEqual(false, updatedPot.Deleted);
            }

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            ClassicAssert.AreEqual("PUT", fakeMessageHandler.Request.Method.ToString());
            ClassicAssert.AreEqual("/pots/pot_00009exampleP0tOxWb/withdraw", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("test_account_123", formCollection["destination_account_id"]);
            ClassicAssert.AreEqual("500", formCollection["amount"]);
            ClassicAssert.AreEqual("a_unique_id", formCollection["dedupe_id"]);
        }
    }
}
