using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Handallo.Global.VerificationService;
using MimeKit;

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
                mail.From = new MailAddress("dulangah2@gmail.com");
                mail.Subject = "Confirmation of Registration from handallo.";
                string Body = message;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("///////////", "/////");
                // smtp.Port = 587;
                //Or your Smtp Email ID and Password
                smtp.Send(mail);




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
