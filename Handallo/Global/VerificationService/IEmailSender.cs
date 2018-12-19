using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.Global.VerificationService
{
    interface IEmailSender
    {
        Task SendEmailAsync(string email,string message);
        Task<IActionResult> SendEmail(string email,string verificode);
    }
}
