using MailKit.Net.Imap;
using System.Net.Http;
using System.Net.Mail;

namespace IdentityProject.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link, string email)
        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpClient = new SmtpClient();

            mail.From = new MailAddress("fatihsaridag26@gmail.com");
            mail.To.Add(email);

            mail.Subject = $"www.sifresifirlama.com::Şifre sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new System.Net.NetworkCredential("fatihsaridag26@gmail.com", "vsziafjpokcmwwac");

            smtpClient.Send(mail);

        }
    }
}
