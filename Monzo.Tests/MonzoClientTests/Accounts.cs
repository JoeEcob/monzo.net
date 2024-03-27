namespace Monzo.Tests.MonzoClientTests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public sealed class Accounts
    {
        [Test]
        public async Task CanGetAccounts()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'accounts': [
                        {
                            'id': 'acc_00009237aqC8c5umZmrRdh',
                            'description': 'Peter Pan\'s Account',
                            'created': '2015-11-13T12:17:42Z',
                            'type': 'uk_retail',
                            'currency': 'GBP',
                            'country_code': 'GB',
                            'sort_code': '040004',
                            'account_number': '12345678',
                            'closed': false,
                            'owners': [
                                {
                                    'user_id': 'user_00009awdawdawdawdg',
                                    'preferred_name': 'Peter Pan',
                                    'preferred_first_name': 'Peter'
                                }
                            ]
                        },
                        {
                            'id': 'acc_00009238aqC8c5umZmrRdh',
                            'description': 'Joint account between user_00009awdawdawdawdr and user_00009awdawdawdawdg',
                            'created': '2019-10-27T19:50:42Z',
                            'type': 'uk_retail_joint',
                            'currency': 'GBP',
                            'country_code': 'GB',
                            'sort_code': '040004',
                            'account_number': '567891234',
                            'closed': true,
                            'owners': [
                                {
                                    'user_id': 'user_00009awdawdawdawdr',
                                    'preferred_name': 'Captain Hook',
                                    'preferred_first_name': 'Hook'
                                },
                                {
                                    'user_id': 'user_00009awdawdawdawdg',
                                    'preferred_name': 'Peter Pan',
                                    'preferred_first_name': 'Peter'
                                }
                            ]
                        },
						{
                            'id': 'acc_00009abcdefghijklmnopq',
                            'closed': false,
                            'created': '2019-10-27T19:50:42Z',
                            'description': 'loan_00009abcdefghijklmnopq',
                            'type': 'uk_loan',
                            'currency': 'GBP',
                            'country_code': 'GB',
                            'owners': [
                                {
                                    'user_id': 'user_00009awdawdawdawdg',
                                    'preferred_name': 'Peter Pan',
                                    'preferred_first_name': 'Peter'
                                }
                            ]
                        }
                    ]
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var accounts = await client.GetAccountsAsync();

                ClassicAssert.AreEqual(3, accounts.Count);
                var singleAccount = accounts[0];
                #region Single Account
                ClassicAssert.AreEqual("acc_00009237aqC8c5umZmrRdh", singleAccount.Id);
                ClassicAssert.AreEqual("Peter Pan's Account", singleAccount.Description);
                ClassicAssert.AreEqual(AccountType.uk_retail, singleAccount.Type);
                ClassicAssert.AreEqual("040004", singleAccount.SortCode);
                ClassicAssert.AreEqual("12345678", singleAccount.AccountNumber);
                ClassicAssert.AreEqual(false, singleAccount.Closed);
                ClassicAssert.AreEqual(1, singleAccount.Owners.Length);

                ClassicAssert.AreEqual("user_00009awdawdawdawdg", singleAccount.Owners[0].Id);
                ClassicAssert.AreEqual("Peter Pan", singleAccount.Owners[0].PreferredName);
                ClassicAssert.AreEqual("Peter", singleAccount.Owners[0].PreferredFirstName);

                ClassicAssert.AreEqual(new DateTime(2015, 11, 13, 12, 17, 42, DateTimeKind.Utc), singleAccount.Created);
                #endregion

                var jointAccount = accounts[1];
                #region Joint Account
                ClassicAssert.AreEqual("acc_00009238aqC8c5umZmrRdh", jointAccount.Id);
                ClassicAssert.AreEqual("Joint account between user_00009awdawdawdawdr and user_00009awdawdawdawdg", jointAccount.Description);
                ClassicAssert.AreEqual(AccountType.uk_retail_joint, jointAccount.Type);
                ClassicAssert.AreEqual("040004", jointAccount.SortCode);
                ClassicAssert.AreEqual("567891234", jointAccount.AccountNumber);
                ClassicAssert.AreEqual(true, jointAccount.Closed);
                ClassicAssert.AreEqual(2, jointAccount.Owners.Length);
                ClassicAssert.AreEqual("user_00009awdawdawdawdr", jointAccount.Owners[0].Id);
                ClassicAssert.AreEqual("Captain Hook", jointAccount.Owners[0].PreferredName);
                ClassicAssert.AreEqual("Hook", jointAccount.Owners[0].PreferredFirstName);
                ClassicAssert.AreEqual("user_00009awdawdawdawdg", jointAccount.Owners[1].Id);
                ClassicAssert.AreEqual("Peter Pan", jointAccount.Owners[1].PreferredName);
                ClassicAssert.AreEqual("Peter", jointAccount.Owners[1].PreferredFirstName);

                ClassicAssert.AreEqual(new DateTime(2019, 10, 27, 19, 50, 42, DateTimeKind.Utc), jointAccount.Created);
                #endregion

                var loan = accounts[2];

                #region Loan
                ClassicAssert.AreEqual("acc_00009abcdefghijklmnopq", loan.Id);
                ClassicAssert.AreEqual(false, loan.Closed);
                ClassicAssert.AreEqual("loan_00009abcdefghijklmnopq", loan.Description);
                ClassicAssert.AreEqual(AccountType.uk_loan, loan.Type);
                ClassicAssert.AreEqual("GBP", loan.Currency);
                ClassicAssert.AreEqual("GB", loan.CountryCode);
                ClassicAssert.AreEqual("user_00009awdawdawdawdg", loan.Owners[0].Id);
                ClassicAssert.AreEqual("Peter Pan", loan.Owners[0].PreferredName);
                ClassicAssert.AreEqual("Peter", loan.Owners[0].PreferredFirstName);

                ClassicAssert.AreEqual(new DateTime(2019, 10, 27, 19, 50, 42, DateTimeKind.Utc), loan.Created);
                #endregion
            }

            ClassicAssert.AreEqual("/accounts", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }
    }
}
