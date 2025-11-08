using System.Net;
using System.Net.Mail;

namespace ComplianceAPI.Helpers
{
    public interface IEmail
    {
        Task Sendemail(string ToEmail, string Name, string UserName, string Password);

        Task<bool> SendEmailRequestProcess(string toEmail, string subject, string body);
    }

    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Email> _logger;

        public Email(IConfiguration config, ILogger<Email> logger)
        {
            this._configuration = config;
            _logger = logger;
        }

        public async Task Sendemail(string ToEmail, string Name, string UserName, string Password)
        {
            try
            {
                var host = _configuration.GetValue<string>("EmailConfig:Host");
                var fromMail = _configuration.GetValue<string>("EmailConfig:FromMail");
                var Gmailusername = _configuration.GetValue<string>("EmailConfig:Username");
                var Gmailpassword = _configuration.GetValue<string>("EmailConfig:Password");

                string body = @"
                    Hi <b> {Name}</b>,<br>
                            Welcome to compliance.<br>
                    User Name: {UserName}<br>
                    Password: {Password}

                ";
                body = body.Replace("{UserName}", UserName);
                body = body.Replace("{Name}", Name);
                body = body.Replace("{Password}", Password);
                const string Subject = "Welcome to Compliance";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential(Gmailusername, Gmailpassword),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = Subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(ToEmail);

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            //catch (Exception ex)
            //{
            //    //throw ex;
            //}
        }

        ///<see cref="IEmail.SendEmailRequestProcess(string, string, string)"/>
        public async Task<bool> SendEmailRequestProcess(string toEmail, string subject, string body)
        {
            try
            {
                _logger.LogInformation("Preparing to send order process email to {ToEmail}", toEmail);

                var host = _configuration.GetValue<string>("EmailConfig:Host");
                var fromMail = _configuration.GetValue<string>("EmailConfig:FromMail");
                var Gmailusername = _configuration.GetValue<string>("EmailConfig:Username");
                var Gmailpassword = _configuration.GetValue<string>("EmailConfig:Password");
                host = host ?? "smtp.gmail.com";
                var smtpClient = new SmtpClient(host)
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, Gmailpassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromMail!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                _logger.LogInformation("Sending order process email to {ToEmail} with subject '{Subject}'", toEmail, subject);
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Order process email sent successfully to {ToEmail}", toEmail);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending order process email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}