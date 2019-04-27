using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoolBytes.Services.Mailer
{
    public class MailgunMailer : IMailer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MailgunMailerOptions _options;
        private readonly ISendValidator _validator;
        private readonly ILogger<MailgunMailer> _logger;

        public MailgunMailer(IHttpClientFactory httpClientFactory, MailgunMailerOptions options, ISendValidator validator, ILogger<MailgunMailer> logger)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _validator = validator;
            _logger = logger;
        }

        public async Task<IMailReport> Send(EmailMessage message)
        {
            var isSendingAllowed = await _validator.IsSendingAllowed(this);

            if (!isSendingAllowed)
            {
                _logger.LogError("Sending not allowed");
                throw new InvalidOperationException("Sending not allowed");
            }

            using (_logger.BeginScope("Start sending message."))
            {
                var httpMessage = CreateMessage();
                SetFormData(message, httpMessage);

                return await Send(httpMessage);
            }
        }

        private HttpRequestMessage CreateMessage()
        {
            var uri = $"{_options.Server}/{_options.Domain}/messages";
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var credentials = $"{_options.Credentials.UserName}:{_options.Credentials.Password}";
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue("basic", base64);

            _logger.LogInformation("Created message.");

            return httpMessage;
        }

        private void SetFormData(EmailMessage message, HttpRequestMessage httpMessage)
        {
            var formDictonary = new Dictionary<string, string>
            {
                {"from", $"{message.From.DisplayName} <{message.From.Email}>"},
                {"to", $"{message.To.DisplayName} <{message.To.Email}>"},
                {"subject", message.Subject },
                {"html", message.Body }
            };

            _logger.LogInformation("Generated formdata.");

            httpMessage.Content = new FormUrlEncodedContent(formDictonary);
        }

        private async Task<IMailReport> Send(HttpRequestMessage httpMessage)
        {
            _logger.LogInformation("Sending message to socket endpoint {endPoint}.", httpMessage.RequestUri);

            HttpResponseMessage response;
            try
            {
                var httpClient = _httpClientFactory.CreateClient(); 
                response = await httpClient.SendAsync(httpMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sending message failed.");
                throw;
            }

            if (response.IsSuccessStatusCode)
            {
                return await Deserialize(response);
            }
            else
            {
                var responseText = await response.Content.ReadAsStringAsync();
                _logger.LogError("Unexpected response, status code {statusCode}, body {responseText}.", response.StatusCode, responseText);

                return new MailReport(isSend: false);
            }
        }

        private async Task<IMailReport> Deserialize(HttpResponseMessage response)
        {
            _logger.LogInformation("Deserializing response body.");
            var responseText = await response.Content.ReadAsStringAsync();

            try
            {
                var mailgunResponse = JsonConvert.DeserializeObject<MailgunResponse>(responseText);

                _logger.LogInformation("Deserializing succeeded. Message {id}.", mailgunResponse.Id);

                return new MailReport(isSend: true, id: mailgunResponse.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deserializing of {responseText} failed.", responseText);
                throw;
            }
        }
    }
}