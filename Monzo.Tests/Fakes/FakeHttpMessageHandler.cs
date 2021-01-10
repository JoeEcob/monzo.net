namespace Monzo.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    internal class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _responseContent;

        public HttpRequestMessage Request;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string responseContent)
        {
            _statusCode = statusCode;
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;

            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = _statusCode,
                Content = new StringContent(_responseContent)
            });
        }

        public async Task<NameValueCollection> GetQueryStringAsync()
        {
            var content = await Request.Content.ReadAsStringAsync();
            return HttpUtility.ParseQueryString(content);
        }
    }
}
