namespace Monzo.Tests.Extensions
{
    using System;
    using NUnit.Framework;
    using Monzo.Extensions;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public sealed class DateTimeExtensionsTests
    {
        [Test]
        public void ToRfc3339String()
        {
            ClassicAssert.AreEqual("2015-04-05T18:01:32Z", new DateTime(2015, 4, 5, 18, 1, 32, DateTimeKind.Utc).ToRfc3339String());
        }
    }
}
