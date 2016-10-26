using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace AwesomeServer.Services.Interfaces
{
    // More on https://github.com/jstedfast/MailKit
    public class EmailSender : IEmailSender
    {
        readonly IOptions<ServerOptions> _serverOptions;

        public EmailSender(IOptions<ServerOptions> serverOptions)
        {
            _serverOptions = serverOptions;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _serverOptions.Value.ProjectName,
                _serverOptions.Value.EmailOptions.Account));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // This will work for a demo gmail account using the following settings:
                // host: smtp.gmail.com
                // port: 465
                // useSsl: true
                // as long as you enable the access for less secure apps for that account.
                // To do so go to https://www.google.com/settings/security/lesssecureapps
                await client.ConnectAsync(
                    _serverOptions.Value.EmailOptions.Server,
                    _serverOptions.Value.EmailOptions.Port,
                    _serverOptions.Value.EmailOptions.SSL);

                await client.AuthenticateAsync(
                    _serverOptions.Value.EmailOptions.Account,
                    _serverOptions.Value.EmailOptions.Password);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}