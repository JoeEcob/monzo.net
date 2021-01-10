namespace Monzo.Tests.Fakes
{
    using System;
    using System.Net;
    using System.Net.Http;

    internal static class FakeHttpClientFactory
    {
        public static (HttpClient httpClient, FakeHttpMessageHandler fakeMessageHandler) Create(HttpStatusCode statusCode, string responseContent)
        {
            var fakeMessageHandler = new FakeHttpMessageHandler(statusCode, responseContent);

            return (new HttpClient(fakeMessageHandler)
            {
                BaseAddress = new Uri("https://localhost")
            }, fakeMessageHandler);
        }
    }
}
