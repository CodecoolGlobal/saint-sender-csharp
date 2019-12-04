using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaintSender.Core.Entities
{
    public class Email
    {
        public Email(string from, string to, string date, string subject, string body, bool read)
        {
            From = from;
            To = to;
            Date = date;
            Subject = subject;
            Body = body;
            Read = read;
        }

        private string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Read{ get; set; }
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
