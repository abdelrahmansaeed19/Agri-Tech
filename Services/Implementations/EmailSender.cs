using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using AgriculturalTech.API.Data.Models;

namespace AgriculturalTech.API.Services.Implementations
{
    public interface IExtendedEmailSender : IEmailSender
    {
        Task SendVerificationCode(ApplicationUser user, string codeType = "");
        Task<bool> VerifyCode(ApplicationUser user, string code);
    }

    public class EmailSender : IExtendedEmailSender
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailSender(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("AgriTech", _config["Email:From"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            using var client = new SmtpClient();
            await client.ConnectAsync(_config["Email:SmtpServer"], int.Parse(_config["Email:Port"]), true);
            await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendVerificationCode(ApplicationUser user, string codeType = "EmailVerification")
        {
            var code = new Random().Next(1000, 9999).ToString();

            await _userManager.SetAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "Code", code);

            var subject = "";
            var message = "";

            if (codeType == "PasswordReset")
            {
                message = $"<p>Your Pssword Reset code is: <b>{code}</b></p>";

                subject = $"Password Reset";
            }
            else
            {
                message = $"<p>Your email verification code is: <b>{code}</b></p>";

                subject = $"Email Verification";
            }

            await SendEmailAsync(user.Email, subject, message);
        }

        public async Task<bool> VerifyCode(ApplicationUser user, string code)
        {
            var storedCode = await _userManager.GetAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "Code");

            if (storedCode == null || storedCode != code)
                return false;

            //user.EmailConfirmed = true;

            //await _userManager.UpdateAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "Code");

            return storedCode == code;
        }
    }
}
