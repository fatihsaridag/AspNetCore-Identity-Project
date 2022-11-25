using MailKit.Net.Imap;
using System.Net.Http;
using System.Net.Mail;

namespace IdentityProject.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link, string email)
        {
            MailMessage mail = new MailMessage();           //Mail messaj sınıfı oluşturduk 

            SmtpClient smtpClient = new SmtpClient();       //Smtp client içerisinde email bilgilerimizi giricez.

            mail.From = new MailAddress("fatihsaridag26@gmail.com");    //Hangi email adresini kullanacagımızı belirttik.
            mail.To.Add(email);                                         // Kime göndericez parametre olarak aldığımız email adresine . 

            mail.Subject = $"www.sifresifirlama.com::Şifre sıfırlama";  //Konuyu belirttik
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";    //Bodyi ekledik
            mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";  //Bodye bu mesajı sonradan dahil ettik
            mail.IsBodyHtml = true;         //Html kodunu true yaptık
            smtpClient.Port = 587;          //Smtp portu
            smtpClient.Host = "smtp.gmail.com";  //Smtp Host bilgisi
            smtpClient.EnableSsl = true;    //Smtp ssl

            smtpClient.Credentials = new System.Net.NetworkCredential("fatihsaridag26@gmail.com", "vsziafjpokcmwwac");  //Smtp de mail doğrulama ayarları

            smtpClient.Send(mail);      //Oluşturduğumuz maili smtp ye gönderdik.

        }
    }
}
