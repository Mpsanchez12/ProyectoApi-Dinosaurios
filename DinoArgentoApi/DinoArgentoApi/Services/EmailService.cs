using DinoArgentoApi.Utils;
using Resend;
using System.Net.Mail;

namespace DinoArgentoApi.Services
{
    public class EmailService
    {
        private readonly IResend _resend;

        public EmailService(IResend resend) { _resend = resend; }

        public async Task Execute(EmailMessage message)
        {
            var res = await _resend.EmailSendAsync(message);
        }

        public async Task SendResetPwdAsync(string userName, string callbackUrl)
        {
            var message = new EmailMessage();
            message.From = "noreply <onboarding@resend.dev>";
            message.To.Add("delivered@resend.dev");
            message.Subject = "Reset Password";

            var data = new { userName = userName, appName = "DinoArgento", resetUrl = callbackUrl };
            message.HtmlBody = HandlebarsHelper.GenerateResetPwdTemplate(data);

            await Execute(message);
        }
    }
}

   
