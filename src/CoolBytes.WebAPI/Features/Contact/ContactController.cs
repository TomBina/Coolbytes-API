using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Contact
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IMailer _mailer;
        private readonly IConfiguration _configuration;

        public ContactController(IMailer mailer, IConfiguration configuration)
        {
            _mailer = mailer;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendEmailCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var from = new EmailAddress(_configuration["Mailgun:Sender:Name"], _configuration["Mailgun:Sender:Email"]);
            var to = new EmailAddress(_configuration["Mailgun:Reciever:Name"], _configuration["Mailgun:Reciever:Email"]);
            var body = $"{command.Name} ({command.Email}) send the following: {command.Message}<br />";
            var message = new EmailMessage(from, to, "Message from website", body);

            var report = await _mailer.Send(message);

            return Ok();
        }
    }
}
