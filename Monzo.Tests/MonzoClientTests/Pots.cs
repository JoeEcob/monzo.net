﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Pots
    {
        [Test]
        public async void CanGetPots()
        {
            var currentAccountId = "this_is_a_current_account";

            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual($"/pots?current_account_id={currentAccountId}", context.Request.Uri.PathAndQuery);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    await context.Response.WriteAsync(
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
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var pots = await client.GetPotsAsync(currentAccountId);

                    Assert.AreEqual(1, pots.Count);

                    var pot = pots[0];
                    Assert.AreEqual("pot_0000123456789", pot.Id);
                    Assert.AreEqual("Rainy Day", pot.Name);
                    Assert.AreEqual("beach_ball", pot.Style);
                    Assert.AreEqual(500, pot.Balance);
                    Assert.AreEqual("GBP", pot.Currency);
                    Assert.AreEqual("this_is_a_current_account", pot.CurrentAccountId);
                    Assert.AreEqual(new DateTime(2017, 12, 1, 23, 00, 26, 256, DateTimeKind.Utc), pot.Created);
                    Assert.AreEqual(new DateTime(2018, 1, 22, 8, 12, 49, 497, DateTimeKind.Utc), pot.Updated);
                    Assert.AreEqual(false, pot.RoundUp);
                    Assert.AreEqual(false, pot.Locked);
                    Assert.AreEqual(false, pot.Deleted);
                }
            }
        }

        [Test]
        public async void CanDepositIntoPot()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);
                    Assert.AreEqual("PUT", context.Request.Method);
                    Assert.AreEqual("/pots/pot_00009exampleP0tOxWb/deposit", context.Request.Uri.PathAndQuery);

                    var formCollection = await context.Request.ReadFormAsync();

                    Assert.AreEqual("test_account_123", formCollection["source_account_id"]);
                    Assert.AreEqual("72932", formCollection["amount"]);
                    Assert.AreEqual("a_unique_id", formCollection["dedupe_id"]);

                    await context.Response.WriteAsync(
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
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var updatedPot = await client.DepositIntoPotAsync("pot_00009exampleP0tOxWb", "test_account_123", 72932, "a_unique_id");

                    Assert.NotNull(updatedPot);

                    Assert.AreEqual("pot_00009exampleP0tOxWb", updatedPot.Id);
                    Assert.AreEqual("Wedding Fund", updatedPot.Name);
                    Assert.AreEqual("beach_ball", updatedPot.Style);
                    Assert.AreEqual(550100, updatedPot.Balance);
                    Assert.AreEqual("GBP", updatedPot.Currency);
                    Assert.AreEqual("test_account_123", updatedPot.CurrentAccountId);
                    Assert.AreEqual(new DateTime(2017, 11, 9, 12, 30, 53, 695, DateTimeKind.Utc), updatedPot.Created);
                    Assert.AreEqual(new DateTime(2018, 2, 26, 7, 12, 4, 925, DateTimeKind.Utc), updatedPot.Updated);
                    Assert.AreEqual(false, updatedPot.Deleted);
                }
            }
        }

        [Test]
        public async void CanWithdrawFromPot()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);
                    Assert.AreEqual("PUT", context.Request.Method);
                    Assert.AreEqual("/pots/pot_00009exampleP0tOxWb/withdraw", context.Request.Uri.PathAndQuery);

                    var formCollection = await context.Request.ReadFormAsync();

                    Assert.AreEqual("test_account_123", formCollection["destination_account_id"]);
                    Assert.AreEqual("500", formCollection["amount"]);
                    Assert.AreEqual("a_unique_id", formCollection["dedupe_id"]);

                    await context.Response.WriteAsync(
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
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var updatedPot = await client.WithdrawFromPotAsync("pot_00009exampleP0tOxWb", "test_account_123", 500, "a_unique_id");

                    Assert.NotNull(updatedPot);

                    Assert.AreEqual("pot_00009exampleP0tOxWb", updatedPot.Id);
                    Assert.AreEqual("Flying Lessons", updatedPot.Name);
                    Assert.AreEqual("blue", updatedPot.Style);
                    Assert.AreEqual(350000, updatedPot.Balance);
                    Assert.AreEqual("GBP", updatedPot.Currency);
                    Assert.AreEqual("test_account_123", updatedPot.CurrentAccountId);
                    Assert.AreEqual(new DateTime(2017, 11, 9, 12, 30, 53, 695, DateTimeKind.Utc), updatedPot.Created);
                    Assert.AreEqual(new DateTime(2018, 2, 26, 7, 12, 4, 925, DateTimeKind.Utc), updatedPot.Updated);
                    Assert.AreEqual(false, updatedPot.Deleted);
                }
            }
        }
    }
}
