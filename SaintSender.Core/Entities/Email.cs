using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SaintSender.Core.Entities
{
    public class Email : BindableBase
    {
        public Email(string id, string from, string to, string date, string subject, string body, bool read)
        {
            Id = id;
            From = from;
            To = to;
            Date = date;
            Subject = subject;
            Body = body;
            Read = read;
        }

        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        private bool _read;
        public bool Read
        {
            get { return _read; }
            set
            {
                SetProperty(ref _read, value);
            }
        }
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
