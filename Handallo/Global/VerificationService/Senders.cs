using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Handallo.Global.VerificationService;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DemoApp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class Senders : IEmailSender
    {


        public async Task SendEmailAsync(string email, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("dulang@gmail.com");
                mail.Subject = "Confirmation of Registration from handallo.";
                string Body = message;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("dulangah2@gmail.com", "Dh.0772646981");
                // smtp.Port = 587;
                //Or your Smtp Email ID and Password
                smtp.Send(mail);




            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public async Task<IActionResult> SendEmail(string email,String verificode)
        {
            var subject = "verificode";
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient("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");
            var from = new EmailAddress("Handallo@gmail.com", "Reset Password");
            var to = new EmailAddress(email);
            var plainTextContent = "//";
            var msg = MailHelper.CreateSingleEmail(from,to, subject, plainTextContent,subject);
            var  response = await client.SendEmailAsync(msg);
            return  new OkObjectResult(response);
        }



        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
