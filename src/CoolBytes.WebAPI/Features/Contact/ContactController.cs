using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Contact
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IMailer _mailer;

        public ContactController(IMailer mailer)
        {
            _mailer = mailer;
        }

        public async Task<IActionResult> Send()
        {
            var from = new EmailAddress("noreply", "noreply@mailapplication.nl");
            var to = new EmailAddress("tom", "tombina@outlook.com");
            var message = new EmailMessage(from, to, "Test", "Hello world!");

            var report = await _mailer.Send(message);

            return Ok(report.Id);
        }
    }
}
