using System;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace Monzo.Tests.MonzoClientTests
{
    [TestFixture]
    public sealed class Attachements
    {
        [Test]
        public async void CanCreateAttachment()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/attachment/register", context.Request.Uri.PathAndQuery);
                    Assert.AreEqual("POST", context.Request.Method);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    var formCollection = await context.Request.ReadFormAsync();
                    Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", formCollection["external_id"]);
                    Assert.AreEqual("image/png", formCollection["file_type"]);
                    Assert.AreEqual("https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", formCollection["file_url"]);

                    await context.Response.WriteAsync(
                        @"{
                            'attachment': {
                                'id': 'attach_00009238aOAIvVqfb9LrZh',
                                'user_id': 'user_00009238aMBIIrS5Rdncq9',
                                'external_id': 'tx_00008zIcpb1TB4yeIFXMzx',
                                'file_url': 'https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png',
                                'file_type': 'image/png',
                                'created': '2015-11-12T18:37:02Z'
                            }
                        }"
                    );
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    var attachment = await client.CreateAttachmentAsync("tx_00008zIcpb1TB4yeIFXMzx", "https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", "image/png");

                    Assert.AreEqual("attach_00009238aOAIvVqfb9LrZh", attachment.Id);
                    Assert.AreEqual("user_00009238aMBIIrS5Rdncq9", attachment.UserId);
                    Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", attachment.ExternalId);
                    Assert.AreEqual("https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", attachment.FileUrl);
                    Assert.AreEqual("image/png", attachment.FileType);
                    Assert.AreEqual(new DateTime(2015, 11, 12, 18, 37, 2, DateTimeKind.Utc), attachment.Created);
                }
            }
        }

        [Test]
        public async void CanDeleteAttachment()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Run(async context =>
                {
                    Assert.AreEqual("/attachment/deregister", context.Request.Uri.PathAndQuery);
                    Assert.AreEqual("POST", context.Request.Method);

                    Assert.AreEqual("Bearer testAccessToken", context.Request.Headers["Authorization"]);

                    var formCollection = await context.Request.ReadFormAsync();
                    Assert.AreEqual("attach_00009238aOAIvVqfb9LrZh", formCollection["id"]);

                    await context.Response.WriteAsync("{}");
                });
            }))
            {
                using (var client = new MonzoClient(server.HttpClient, "testAccessToken"))
                {
                    await client.DeleteAttachmentAsync("attach_00009238aOAIvVqfb9LrZh");
                }
            }
        }
    }
}
