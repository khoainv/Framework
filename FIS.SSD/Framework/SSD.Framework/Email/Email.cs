#region

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

#endregion

namespace SSD.Framework.Email
{
    public class Email : Singleton<Email>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="user">info@ugtrad.com</param>
        /// <param name="pwd">abcde12345-</param>
        public void SendMailByGMail(MailMessage Message, string user, string pwd)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(user, pwd);
            client.Send(Message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress">khoainv@ugroup.vn</param>
        /// <param name="toAddress">khoainv@ugroup.vn</param>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <param name="user">info@ugtrad.com</param>
        /// <param name="pwd">abcde12345-</param>
        public void SendMailByGMail(string fromAddress, string toAddress, string subject, string msg, string user, string pwd)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(fromAddress);
            Message.To.Add(new MailAddress(toAddress));
            Message.Subject = subject;
            Message.Body = msg;
            SendMailByGMail(Message, user, pwd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="smtp">info@ugtrad.com/abcde12345-</param>
        public void SendMail(MailMessage Message, SmtpData smtp)
        {
            SmtpClient client = new SmtpClient();
            client.Host = smtp.Host;
            client.Port = smtp.Port;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
            client.Send(Message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="smtp">info@ugtrad.com/abcde12345-</param>
        public async Task SendMailAsync(MailMessage Message, SmtpData smtp)
        {
            SmtpClient client = new SmtpClient();
            client.Host = smtp.Host;
            client.Port = smtp.Port;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
            await client.SendMailAsync(Message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="smtp">info@ugtrad.com/abcde12345-</param>
        public async Task SendMailAsync(MailMessage Message, SmtpData smtp, SendCompletedEventHandler Client_SendCompleted)
        {
            SmtpClient client = new SmtpClient();
            client.Host = smtp.Host;
            client.Port = smtp.Port;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);

            client.SendCompleted += Client_SendCompleted;

            await client.SendMailAsync(Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress">khoainv@ugroup.vn</param>
        /// <param name="toAddress">khoainv@ugroup.vn</param>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <param name="smtp">info@ugtrad.com/abcde12345-</param>
        public async Task SendMailAsync(string fromAddress, string toAddress, string subject, string msg, SmtpData smtp)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(fromAddress);
            Message.To.Add(new MailAddress(toAddress));
            Message.Subject = subject;
            Message.Body = msg;
            await SendMailAsync(Message, smtp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress">khoainv@ugroup.vn</param>
        /// <param name="toAddress">khoainv@ugroup.vn</param>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <param name="smtp">info@ugtrad.com/abcde12345-</param>
        public void SendMail(string fromAddress, string toAddress, string subject, string msg, SmtpData smtp)
        {
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(fromAddress);
            Message.To.Add(new MailAddress(toAddress));
            Message.Subject = subject;
            Message.Body = msg;
            SendMail(Message, smtp);
        }
    }
}
