namespace Monzo.Tests.Messages
{
    using Monzo.Messages;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public sealed class MerchantJsonConverterTests
    {
        [Test]
        public void Null()
        {
            var message = JsonConvert.DeserializeObject<TestMessage>("{'merchant': null}");
            ClassicAssert.IsNull(message.Merchant);
        }

        [Test]
        public void Missing()
        {
            var message = JsonConvert.DeserializeObject<TestMessage>("{}");
            ClassicAssert.IsNull(message.Merchant);
        }

        [Test]
        public void String()
        {
            var message = JsonConvert.DeserializeObject<TestMessage>("{'merchant': '1234'}");
            ClassicAssert.AreEqual("1234", message.Merchant.Id);
        }

        [Test]
        public void Object()
        {
            var message = JsonConvert.DeserializeObject<TestMessage>("{'merchant': {'id':'1234', 'name':'testMerchant'}}");
            ClassicAssert.AreEqual("1234", message.Merchant.Id);
            ClassicAssert.AreEqual("testMerchant", message.Merchant.Name);
        }

        private sealed class TestMessage
        {
            [JsonProperty("merchant")]
            [JsonConverter(typeof(MerchantJsonConverter))]
            public Merchant Merchant { get; set; }
        }
    }
}
