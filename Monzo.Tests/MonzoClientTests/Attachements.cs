namespace Monzo.Tests.MonzoClientTests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Monzo.Tests.Fakes;
    using NUnit.Framework;

    [TestFixture]
    public sealed class Attachements
    {
        [Test]
        public async Task CanCreateAttachment()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK,
                @"{
                    'attachment': {
                        'id': 'attach_00009238aOAIvVqfb9LrZh',
                        'user_id': 'user_00009238aMBIIrS5Rdncq9',
                        'external_id': 'tx_00008zIcpb1TB4yeIFXMzx',
                        'file_url': 'https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png',
                        'file_type': 'image/png',
                        'created': '2015-11-12T18:37:02Z'
                    }
                }");

                using (var client = new MonzoClient(httpClient, "testAccessToken"))
                {
                    var attachment = await client.CreateAttachmentAsync("tx_00008zIcpb1TB4yeIFXMzx", "https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", "image/png");

                    Assert.AreEqual("attach_00009238aOAIvVqfb9LrZh", attachment.Id);
                    Assert.AreEqual("user_00009238aMBIIrS5Rdncq9", attachment.UserId);
                    Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", attachment.ExternalId);
                    Assert.AreEqual("https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", attachment.FileUrl);
                    Assert.AreEqual("image/png", attachment.FileType);
                    Assert.AreEqual(new DateTime(2015, 11, 12, 18, 37, 2, DateTimeKind.Utc), attachment.Created);
                }

                Assert.AreEqual("/attachment/register", fakeMessageHandler.Request.RequestUri.PathAndQuery);
                Assert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());

                Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());

                var formCollection = await fakeMessageHandler.GetQueryStringAsync();

                Assert.AreEqual("tx_00008zIcpb1TB4yeIFXMzx", formCollection["external_id"]);
                Assert.AreEqual("image/png", formCollection["file_type"]);
                Assert.AreEqual("https://s3-eu-west-1.amazonaws.com/mondo-image-uploads/user_00009237hliZellUicKuG1/LcCu4ogv1xW28OCcvOTL-foo.png", formCollection["file_url"]);
        }

        [Test]
        public async Task CanDeleteAttachment()
        {
            var (httpClient, fakeMessageHandler) = FakeHttpClientFactory.Create(HttpStatusCode.OK, "{}");

            using (var client = new MonzoClient(httpClient, "testAccessToken"))
            {
                await client.DeleteAttachmentAsync("attach_00009238aOAIvVqfb9LrZh");
            }

            Assert.AreEqual("/attachment/deregister", fakeMessageHandler.Request.RequestUri.PathAndQuery);
            Assert.AreEqual("POST", fakeMessageHandler.Request.Method.ToString());

            Assert.AreEqual("Bearer testAccessToken", fakeMessageHandler.Request.Headers.Authorization.ToString());

            var formCollection = await fakeMessageHandler.GetQueryStringAsync();

            Assert.AreEqual("attach_00009238aOAIvVqfb9LrZh", formCollection["id"]);
        }
    }
}
