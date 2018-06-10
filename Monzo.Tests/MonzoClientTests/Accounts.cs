using System;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Accounts
    {
        [Test]
        public async void CanGetAccounts()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/accounts", context.Request.Uri.PathAndQuery);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync(
                        @"{
                            'accounts': [
                                {
                                    'id': 'acc_00009237aqC8c5umZmrRdh',
                                    'description': 'Peter Pan\'s Account',
                                    'created': '2015-11-13T12:17:42Z',
                                    'type': 'uk_retail',
                                    'sort_code': '040004',
                                    'account_number': '12345678',
                                    'closed': false
                                }
                            ]
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var accounts = await client.GetAccountsAsync();

                    Assert.AreEqual(1, accounts.Count);
                    Assert.AreEqual("acc_00009237aqC8c5umZmrRdh", accounts[0].Id);
                    Assert.AreEqual("Peter Pan's Account", accounts[0].Description);
                    Assert.AreEqual(AccountType.uk_retail, accounts[0].Type);
                    Assert.AreEqual("040004", accounts[0].SortCode);
                    Assert.AreEqual("12345678", accounts[0].AccountNumber);
                    Assert.AreEqual(false, accounts[0].Closed);
                    Assert.AreEqual(new DateTime(2015, 11, 13, 12, 17, 42, DateTimeKind.Utc), accounts[0].Created);
                }
            }
        }
    }
}
