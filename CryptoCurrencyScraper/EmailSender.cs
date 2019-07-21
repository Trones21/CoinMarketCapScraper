using System;
using System.Net.Mail;

namespace CryptoCurrencyScraperFirst
{
    public class EmailSender
    {
       
        public void SendMail(string Subject, string MessageBody)
        {
            try
            {
                MailMessage mail = new MailMessage();

                var SmtpServer = new SmtpClient("smtp.mail.yahoo.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("trones.mailserver@yahoo.com", "myMailServer1"),
  
                };

                mail.From = new MailAddress("trones.mailserver@yahoo.com");
                mail.To.Add("trones.cc.alerts@gmail.com");
                mail.Subject = Subject;
                mail.Body = MessageBody;
                             
                SmtpServer.Send(mail);

                Console.WriteLine("Mail Sent");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        
        }

    }
}
