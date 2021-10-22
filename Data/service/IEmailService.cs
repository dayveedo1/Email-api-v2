using Email.Data.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Data.service
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailInfo emailInfo);

        //Task SendEmailTemplateAsync(EmailSource emailSource);
    }
}
