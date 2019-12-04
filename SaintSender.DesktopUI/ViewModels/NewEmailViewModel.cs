using Google.Apis.Gmail.v1.Data;
using SaintSender.Core.Entities;
using SaintSender.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.DesktopUI.ViewModels
{
    public class NewEmailViewModel : BindableBase
    {
        private EmailService emailService;
        private string _from = "me";
        public string Body { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }

        public NewEmailViewModel(EmailService emailService)
        {
            this.emailService = emailService;
        }

        public void SendEmail()
        {
            string plainText = $"To: {To}\r\n" +
                               $"Subject: {Subject}\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                               $"<p>{Body}<p>";

            Message msg = new Message();
            msg.Raw = Base64UrlEncode(plainText.ToString());
            emailService.SendMail(msg);
        }
        private string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}
