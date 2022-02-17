namespace Monzo.Tests.MonzoClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;

    [TestFixture]
    public sealed class Transactions
    {
        [Test]
        public async Task CanGetTransactions()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transactions': [
                        {
                            'account_balance': 13013,
                            'amount': -510,
                            'created': '2015-08-22T12:20:18Z',
                            'currency': 'GBP',
                            'description': 'THE DE BEAUVOIR DELI C LONDON        GBR',
                            'id': 'tx_00008zIcpb1TB4yeIFXMzx',
                            'merchant': 'merch_00008zIcpbAKe8shBxXUtl',
                            'metadata': {},
                            'notes': 'Salmon sandwich 🍞',
                            'is_load': false,
                            'settled': true,
                            'category': 'eating_out'
                        },
                        {
                            'account_balance': 12334,
                            'amount': -679,
                            'created': '2015-08-23T16:15:03Z',
                            'currency': 'GBP',
                            'description': 'VUE BSL LTD            ISLINGTON     GBR',
                            'id': 'tx_00008zL2INM3xZ41THuRF3',
                            'merchant': 'merch_00008z6uFVhVBcaZzSQwCX',
                            'metadata': {},
                            'notes': '',
                            'is_load': false,
                            'settled': true,
                            'category': 'eating_out'
                        },
                    ]
                }");


            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transactions = await client.GetTransactionsAsync("1");

                Assert.AreEqual(2, transactions.Count);

                Assert.AreEqual(13013, transactions[0].AccountBalance);
                Assert.AreEqual(-510, transactions[0].Amount);
                Assert.AreEqual(new DateTime(2015, 08, 22, 12, 20, 18, DateTimeKind.Utc), transactions[0].Created);
                Assert.AreEqual("GBP", transactions[0].Currency);
                Assert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transactions[0].Description);
                Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transactions[0].Id);
                Assert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transactions[0].Merchant.Id);
                Assert.AreEqual(new Dictionary<string, string>(), transactions[0].Metadata);
                Assert.AreEqual("Salmon sandwich 🍞", transactions[0].Notes);
                Assert.IsFalse(transactions[0].IsLoad);
                Assert.AreEqual("eating_out", transactions[0].Category);
            }

            Assert.AreEqual("/transactions?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanGetTransactionsPaginated()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transactions': [
                        {
                            'account_balance': 13013,
                            'amount': -510,
                            'created': '2015-08-22T12:20:18Z',
                            'currency': 'GBP',
                            'description': 'THE DE BEAUVOIR DELI C LONDON        GBR',
                            'id': 'tx_00008zIcpb1TB4yeIFXMzx',
                            'merchant': 'merch_00008zIcpbAKe8shBxXUtl',
                            'metadata': {},
                            'notes': 'Salmon sandwich 🍞',
                            'is_load': false,
                            'settled': true,
                            'category': 'eating_out'
                        },
                        {
                            'account_balance': 12334,
                            'amount': -679,
                            'created': '2015-08-23T16:15:03Z',
                            'currency': 'GBP',
                            'description': 'VUE BSL LTD            ISLINGTON     GBR',
                            'id': 'tx_00008zL2INM3xZ41THuRF3',
                            'merchant': 'merch_00008z6uFVhVBcaZzSQwCX',
                            'metadata': {},
                            'notes': '',
                            'is_load': false,
                            'settled': true,
                            'category': 'eating_out'
                        },
                    ]
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transactions = await client.GetTransactionsAsync("1", null, null, new PaginationOptions { SinceTime = new DateTime(2015, 4, 5, 18, 1, 32, DateTimeKind.Utc), Limit = 40, BeforeTime = new DateTime(2015, 12, 25, 18, 1, 32, DateTimeKind.Utc) });

                Assert.AreEqual(2, transactions.Count);

                Assert.AreEqual(13013, transactions[0].AccountBalance);
                Assert.AreEqual(-510, transactions[0].Amount);
                Assert.AreEqual(new DateTime(2015, 08, 22, 12, 20, 18, DateTimeKind.Utc), transactions[0].Created);
                Assert.AreEqual("GBP", transactions[0].Currency);
                Assert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transactions[0].Description);
                Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transactions[0].Id);
                Assert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transactions[0].Merchant.Id);
                Assert.AreEqual(new Dictionary<string, string>(), transactions[0].Metadata);
                Assert.AreEqual("Salmon sandwich 🍞", transactions[0].Notes);
                Assert.IsFalse(transactions[0].IsLoad);
                Assert.AreEqual("eating_out", transactions[0].Category);
            }

            Assert.AreEqual("/transactions?account_id=1&limit=40&since=2015-04-05T18:01:32Z&before=2015-12-25T18:01:32Z", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanGetTransaction()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transaction': {
                        'account_balance': 13013,
                        'amount': -510,
                        'created': '2015-08-22T12:20:18Z',
                        'currency': 'GBP',
                        'description': 'THE DE BEAUVOIR DELI C LONDON        GBR',
                        'id': 'tx_00008zIcpb1TB4yeIFXMzx',
                        'merchant': 'merch_00008zIcpbAKe8shBxXUtl',
                        'metadata': {},
                        'notes': 'Salmon sandwich 🍞',
                        'is_load': false,
                        'settled': true
                    }
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transaction = await client.GetTransactionAsync("1");

                Assert.AreEqual(13013, transaction.AccountBalance);
                Assert.AreEqual(-510, transaction.Amount);
                Assert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Created);
                Assert.AreEqual("GBP", transaction.Currency);
                Assert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transaction.Description);
                Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transaction.Id);
                Assert.AreEqual(new Dictionary<string, string>(), transaction.Metadata);
                Assert.AreEqual("Salmon sandwich 🍞", transaction.Notes);
                Assert.IsFalse(transaction.IsLoad);

                Assert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transaction.Merchant.Id);
            }


            Assert.AreEqual("/transactions/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanGetTransactionExpanded()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transaction': {
                        'account_balance': 13013,
                        'amount': -510,
                        'created': '2015-08-22T12:20:18Z',
                        'currency': 'GBP',
                        'description': 'THE DE BEAUVOIR DELI C LONDON        GBR',
                        'id': 'tx_00008zIcpb1TB4yeIFXMzx',
                        'merchant': {
                            'address': {
                                'address': '98 Southgate Road',
                                'city': 'London',
                                'country': 'GB',
                                'latitude': 51.54151,
                                'longitude': -0.08482400000002599,
                                'postcode': 'N1 3JD',
                                'region': 'Greater London'
                            },
                            'created': '2015-08-22T12:20:18Z',
                            'group_id': 'grp_00008zIcpbBOaAr7TTP3sv',
                            'id': 'merch_00008zIcpbAKe8shBxXUtl',
                            'logo': 'https://pbs.twimg.com/profile_images/527043602623389696/68_SgUWJ.jpeg',
                            'emoji': '🍞',
                            'name': 'The De Beauvoir Deli Co.',
                            'category': 'eating_out'
                        },
                        'metadata': {},
                        'notes': 'Salmon sandwich 🍞',
                        'is_load': false,
                        'settled': true
                    }
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transaction = await client.GetTransactionAsync("1", "merchant");

                Assert.AreEqual(13013, transaction.AccountBalance);
                Assert.AreEqual(-510, transaction.Amount);
                Assert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Created);
                Assert.AreEqual("GBP", transaction.Currency);
                Assert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transaction.Description);
                Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transaction.Id);
                Assert.AreEqual(new Dictionary<string, string>(), transaction.Metadata);
                Assert.AreEqual("Salmon sandwich 🍞", transaction.Notes);
                Assert.IsFalse(transaction.IsLoad);

                Assert.AreEqual("98 Southgate Road", transaction.Merchant.Address.Address);
                Assert.AreEqual("London", transaction.Merchant.Address.City);
                Assert.AreEqual(51.54151, transaction.Merchant.Address.Latitude);
                Assert.AreEqual(-0.08482400000002599, transaction.Merchant.Address.Longitude);
                Assert.AreEqual("N1 3JD", transaction.Merchant.Address.Postcode);
                Assert.AreEqual("Greater London", transaction.Merchant.Address.Region);

                Assert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Merchant.Created);
                Assert.AreEqual("grp_00008zIcpbBOaAr7TTP3sv", transaction.Merchant.GroupId);
                Assert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transaction.Merchant.Id);
                Assert.AreEqual("https://pbs.twimg.com/profile_images/527043602623389696/68_SgUWJ.jpeg", transaction.Merchant.Logo);
                Assert.AreEqual("🍞", transaction.Merchant.Emoji);
                Assert.AreEqual("The De Beauvoir Deli Co.", transaction.Merchant.Name);
                Assert.AreEqual("eating_out", transaction.Merchant.Category);
            }

            Assert.AreEqual("/transactions/1?expand[]=merchant", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanGetBankTransfer()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transaction': {
                        'id': 'tx_0000000000000000000001',
                        'created': '2017-11-11T14:40:22.354Z',
                        'description': 'Monzo',
                        'amount': 10000,
                        'fees': {},
                        'currency': 'GBP',
                        'merchant': null,
                        'notes': 'Monzo',
                        'metadata': {
                            'faster_payment': 'true',
                            'insertion': 'entryset_0000000000000000000001',
                            'notes': 'Monzo',
                            'trn': '000000000000000000'
                        },
                        'labels': null,
                        'account_balance': 0,
                        'attachments': [],
                        'international': null,
                        'category': 'general',
                        'is_load': false,
                        'settled': '2017-11-13T07:00:00Z',
                        'local_amount': 10000,
                        'local_currency': 'GBP',
                        'updated': '2017-11-11T14:40:22.423Z',
                        'account_id': 'acc_0000000000000000000000',
                        'user_id': '',
                        'counterparty': {
                            'account_number': '12345678',
                            'name': 'Paddington Bear',
                            'sort_code': '040004',
                            'user_id': 'anonuser_0000000000000000000001'
                        },
                        'scheme': 'payport_faster_payments',
                        'dedupe_id': 'payport-faster-payments:inbound:000000000000000000',
                        'originator': false,
                        'include_in_spending': false,
                        'can_be_excluded_from_breakdown': false
                    }
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transaction = await client.GetTransactionAsync("1", "merchant");

                Assert.AreEqual(10000, transaction.Amount);
                Assert.AreEqual("general", transaction.Category);
                Assert.AreEqual(false, transaction.IncludeInSpending);
                Assert.AreEqual(false, transaction.IsLoad);
                Assert.AreEqual("12345678", transaction.CounterParty.AccountNumber);
                Assert.AreEqual("Paddington Bear", transaction.CounterParty.Name);
                Assert.AreEqual("040004", transaction.CounterParty.SortCode);
                Assert.AreEqual("anonuser_0000000000000000000001", transaction.CounterParty.UserId);
            }

            Assert.AreEqual("/transactions/1?expand[]=merchant", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
        }

        [Test]
        public async Task CanAnnotateTransaction()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'transaction': {
                        'account_balance': 12334,
                        'amount': -679,
                        'created': '2015-08-23T16:15:03Z',
                        'currency': 'GBP',
                        'description': 'VUE BSL LTD            ISLINGTON     GBR',
                        'id': 'tx_00008zL2INM3xZ41THuRF3',
                        'merchant': 'merch_00008z6uFVhVBcaZzSQwCX',
                        'metadata': {
                            'foo': 'bar'
                        },
                        'notes': '',
                        'is_load': false,
                        'settled': true,
                        'category': 'eating_out'
                    }
                }");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                var transaction = await client.AnnotateTransactionAsync("1", new Dictionary<string, string> { { "key1", "value1" }, { "key2", "" } });

                Assert.AreEqual("foo", transaction.Metadata.First().Key);
                Assert.AreEqual("bar", transaction.Metadata.First().Value);
            }

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            Assert.AreEqual("PATCH", fakeMessageHandler.Request.Method.ToString());
            Assert.AreEqual("/transactions/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            Assert.AreEqual("value1", formCollection["metadata[key1]"]);
            Assert.AreEqual("", formCollection["metadata[key2]"]);
        }
    }
}
