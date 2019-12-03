using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Entities
{
    public class Email
    {
        public Email(string from, string to, string date, string subject, string body)
        {
            From = from;
            To = to;
            Date = date;
            Subject = subject;
            Body = body;
        }

        private string From { get; set; }
        private string To { get; set; }
        private string Date { get; set; }
        private string Subject { get; set; }
        private string Body { get; set; }

        private List<FileInfo> Attachments { get; set; }

        public override string ToString()
        {
            return "From: " + From + "\n" +
                   "To: " + To + "\n" +
                   "Date: " + Date + "\n" +
                   "Subject: " + Subject + "\n" +
                   "Body: " + Body;
        }
    }
}
