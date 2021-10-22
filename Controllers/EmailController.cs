using Email.Data.model;
using Email.Data.service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendMail([FromForm] EmailInfo emailInfo)
        {
            try
            {
                await emailService.SendEmailAsync(emailInfo);
                return Ok();

            } catch (Exception ex)
            {
                throw;
            }
        }
    }
}
