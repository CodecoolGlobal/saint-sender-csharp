using System;
using System.Collections.Generic;
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
            Opened = false;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Opened { get; set; }

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
