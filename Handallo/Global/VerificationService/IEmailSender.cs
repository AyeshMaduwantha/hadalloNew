using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Global.VerificationService
{
    interface IEmailSender
    {
        Task SendEmailAsync(string email,string message);
    }
}
