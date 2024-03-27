namespace Monzo.Tests.MonzoClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

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

                ClassicAssert.AreEqual(2, transactions.Count);

                ClassicAssert.AreEqual(13013, transactions[0].AccountBalance);
                ClassicAssert.AreEqual(-510, transactions[0].Amount);
                ClassicAssert.AreEqual(new DateTime(2015, 08, 22, 12, 20, 18, DateTimeKind.Utc), transactions[0].Created);
                ClassicAssert.AreEqual("GBP", transactions[0].Currency);
                ClassicAssert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transactions[0].Description);
                ClassicAssert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transactions[0].Id);
                ClassicAssert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transactions[0].Merchant.Id);
                ClassicAssert.AreEqual(new Dictionary<string, string>(), transactions[0].Metadata);
                ClassicAssert.AreEqual("Salmon sandwich 🍞", transactions[0].Notes);
                ClassicAssert.IsFalse(transactions[0].IsLoad);
                ClassicAssert.AreEqual("eating_out", transactions[0].Category);
            }

            ClassicAssert.AreEqual("/transactions?account_id=1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
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
                var transactions = await client.GetTransactionsAsync("1", null, new PaginationOptions { SinceTime = new DateTime(2015, 4, 5, 18, 1, 32, DateTimeKind.Utc), Limit = 40, BeforeTime = new DateTime(2015, 12, 25, 18, 1, 32, DateTimeKind.Utc) });

                ClassicAssert.AreEqual(2, transactions.Count);

                ClassicAssert.AreEqual(13013, transactions[0].AccountBalance);
                ClassicAssert.AreEqual(-510, transactions[0].Amount);
                ClassicAssert.AreEqual(new DateTime(2015, 08, 22, 12, 20, 18, DateTimeKind.Utc), transactions[0].Created);
                ClassicAssert.AreEqual("GBP", transactions[0].Currency);
                ClassicAssert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transactions[0].Description);
                ClassicAssert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transactions[0].Id);
                ClassicAssert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transactions[0].Merchant.Id);
                ClassicAssert.AreEqual(new Dictionary<string, string>(), transactions[0].Metadata);
                ClassicAssert.AreEqual("Salmon sandwich 🍞", transactions[0].Notes);
                ClassicAssert.IsFalse(transactions[0].IsLoad);
                ClassicAssert.AreEqual("eating_out", transactions[0].Category);
            }

            ClassicAssert.AreEqual("/transactions?account_id=1&limit=40&since=2015-04-05T18:01:32Z&before=2015-12-25T18:01:32Z", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
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

                ClassicAssert.AreEqual(13013, transaction.AccountBalance);
                ClassicAssert.AreEqual(-510, transaction.Amount);
                ClassicAssert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Created);
                ClassicAssert.AreEqual("GBP", transaction.Currency);
                ClassicAssert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transaction.Description);
                ClassicAssert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transaction.Id);
                ClassicAssert.AreEqual(new Dictionary<string, string>(), transaction.Metadata);
                ClassicAssert.AreEqual("Salmon sandwich 🍞", transaction.Notes);
                ClassicAssert.IsFalse(transaction.IsLoad);

                ClassicAssert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transaction.Merchant.Id);
            }


            ClassicAssert.AreEqual("/transactions/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
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

                ClassicAssert.AreEqual(13013, transaction.AccountBalance);
                ClassicAssert.AreEqual(-510, transaction.Amount);
                ClassicAssert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Created);
                ClassicAssert.AreEqual("GBP", transaction.Currency);
                ClassicAssert.AreEqual("THE DE BEAUVOIR DELI C LONDON        GBR", transaction.Description);
                ClassicAssert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", transaction.Id);
                ClassicAssert.AreEqual(new Dictionary<string, string>(), transaction.Metadata);
                ClassicAssert.AreEqual("Salmon sandwich 🍞", transaction.Notes);
                ClassicAssert.IsFalse(transaction.IsLoad);

                ClassicAssert.AreEqual("98 Southgate Road", transaction.Merchant.Address.Address);
                ClassicAssert.AreEqual("London", transaction.Merchant.Address.City);
                ClassicAssert.AreEqual(51.54151, transaction.Merchant.Address.Latitude);
                ClassicAssert.AreEqual(-0.08482400000002599, transaction.Merchant.Address.Longitude);
                ClassicAssert.AreEqual("N1 3JD", transaction.Merchant.Address.Postcode);
                ClassicAssert.AreEqual("Greater London", transaction.Merchant.Address.Region);

                ClassicAssert.AreEqual(new DateTime(2015, 8, 22, 12, 20, 18, DateTimeKind.Utc), transaction.Merchant.Created);
                ClassicAssert.AreEqual("grp_00008zIcpbBOaAr7TTP3sv", transaction.Merchant.GroupId);
                ClassicAssert.AreEqual("merch_00008zIcpbAKe8shBxXUtl", transaction.Merchant.Id);
                ClassicAssert.AreEqual("https://pbs.twimg.com/profile_images/527043602623389696/68_SgUWJ.jpeg", transaction.Merchant.Logo);
                ClassicAssert.AreEqual("🍞", transaction.Merchant.Emoji);
                ClassicAssert.AreEqual("The De Beauvoir Deli Co.", transaction.Merchant.Name);
                ClassicAssert.AreEqual("eating_out", transaction.Merchant.Category);
            }

            ClassicAssert.AreEqual("/transactions/1?expand[]=merchant", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
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

                ClassicAssert.AreEqual(10000, transaction.Amount);
                ClassicAssert.AreEqual("general", transaction.Category);
                ClassicAssert.AreEqual(false, transaction.IncludeInSpending);
                ClassicAssert.AreEqual(false, transaction.IsLoad);
                ClassicAssert.AreEqual("12345678", transaction.CounterParty.AccountNumber);
                ClassicAssert.AreEqual("Paddington Bear", transaction.CounterParty.Name);
                ClassicAssert.AreEqual("040004", transaction.CounterParty.SortCode);
                ClassicAssert.AreEqual("anonuser_0000000000000000000001", transaction.CounterParty.UserId);
            }

            ClassicAssert.AreEqual("/transactions/1?expand[]=merchant", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
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

                ClassicAssert.AreEqual("foo", transaction.Metadata.First().Key);
                ClassicAssert.AreEqual("bar", transaction.Metadata.First().Value);
            }

            ClassicAssert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());
            ClassicAssert.AreEqual("PATCH", fakeMessageHandler.Request.Method.ToString());
            ClassicAssert.AreEqual("/transactions/1", fakeMessageHandler.Request.RequestUri.PathAndQuery);

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            ClassicAssert.AreEqual("value1", formCollection["metadata[key1]"]);
            ClassicAssert.AreEqual("", formCollection["metadata[key2]"]);
        }
    }
}
