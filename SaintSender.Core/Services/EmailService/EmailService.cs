using System;
using System.Collections.Generic;
using System.Text;
using GmailQuickstart;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using SaintSender.Core.Entities;

namespace SaintSender.Core.Services
{
    public partial class EmailService
    {
        private readonly GmailService _gmail;

        public EmailService()
        {
            _gmail = Gmail.GetService();
        }

        public List<Email> GetEmails(bool allEmails, long timeStamp)
        {

            if (allEmails)
            {
                return GetAllEmails();
            }

            return GetLatestEmails(timeStamp);
        }

        private List<Email> GetAllEmails()
        {
            List<Email> emails = new List<Email>();

            UsersResource.MessagesResource.ListRequest messageRequest = _gmail.Users.Messages.List("me");

            messageRequest.MaxResults = 20;
            messageRequest.IncludeSpamTrash = true;
            messageRequest.LabelIds = "INBOX";

            ListMessagesResponse messageResponse = messageRequest.Execute();

            foreach (Message message in messageResponse.Messages)
            {
                Message messageInfo = _gmail.Users.Messages.Get("me", message.Id).Execute();

                bool read = true;

                IList<string> labels = messageInfo.LabelIds;

                if (labels.Contains("UNREAD"))
                {
                    read = false;
                }

                Email email = BuildEmail(messageInfo, message.Id, read);
                emails.Add(email);
            }

            return emails;
        }

        private List<Email> GetLatestEmails(long unixTime)
        {
            List<Email> emails = new List<Email>();

            UsersResource.MessagesResource.ListRequest messageRequest = _gmail.Users.Messages.List("me");

            messageRequest.MaxResults = 10;
            messageRequest.IncludeSpamTrash = true;
            messageRequest.LabelIds = "INBOX";
            messageRequest.Q = $"after:{unixTime}";

            ListMessagesResponse messageResponse = messageRequest.Execute();
            if (messageResponse.Messages != null)
            {
                foreach (Message message in messageResponse.Messages)
                {
                    Message messageInfo = _gmail.Users.Messages.Get("me", message.Id).Execute();

                    bool read = true;

                    IList<string> labels = messageInfo.LabelIds;

                    if (labels.Contains("UNREAD"))
                    {
                        read = false;
                    }

                    Email email = BuildEmail(messageInfo, message.Id, read);
                    emails.Add(email);
                }
            }
            return emails;
        }

    }
}
