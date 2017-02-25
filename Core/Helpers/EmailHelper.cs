using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SW.Core.Helpers
{
    public class EmailHelper
    {
        private const string EMAIL_REGEX = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public static void SendEmail(string email, string emailHtml, string emailSubject)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = AppSettingsHelper.GetAppSetting("Smtp");
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(AppSettingsHelper.GetAppSetting("SmtpUserName"),
                AppSettingsHelper.GetAppSetting("SmtpPassword"));

            mail.To.Add(email);
            mail.From = new MailAddress(AppSettingsHelper.GetAppSetting("EmailFrom"));
            mail.Body = emailHtml;
            mail.Subject = emailSubject;
            mail.IsBodyHtml = true;

            client.Send(mail);
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, EMAIL_REGEX);
        }
    }
}
