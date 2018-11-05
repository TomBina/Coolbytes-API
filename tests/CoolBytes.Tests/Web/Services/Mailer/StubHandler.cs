using CoolBytes.WebAPI.Services.Mailer;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.Tests.Web.Services.Mailer
{
    public class StubHandler : HttpMessageHandler
    {
        private readonly string _response;

        public StubHandler()
        {
            var response = new MailgunResponse() { Id = "0001", Message = "Success" };
            _response = JsonConvert.SerializeObject(response);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_response),
                RequestMessage = request
            };


            return Task.FromResult(message);
        }
    }
}
