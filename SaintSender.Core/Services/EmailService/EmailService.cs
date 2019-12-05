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
        private static GmailService _gmail;

        public EmailService()
        {
            _gmail = Gmail.GetService();
        }

        public List<Email> GetEmails(bool recentOnly, long timeStamp)
        {
            List<Email> emails = new List<Email>();
            UsersResource.MessagesResource.ListRequest messageRequest = _gmail.Users.Messages.List("me");

            messageRequest.MaxResults = 30;
            messageRequest.IncludeSpamTrash = true;
            messageRequest.LabelIds = "INBOX";

            if (recentOnly)
            {
                messageRequest.Q = $"after:{timeStamp}";
            }

            return GetEmailsResponse(messageRequest, emails);
        }

        private List<Email> GetEmailsResponse(UsersResource.MessagesResource.ListRequest request, List<Email> emails)
        {
            ListMessagesResponse messageResponse = request.Execute();

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

        public Message SendMail(Message message)
        {
            var result = _gmail.Users.Messages.Send(message, "me").Execute();
            return result;
        }

        public string GetOwnEmail()
        {
            var me = _gmail.Users.GetProfile("me").Execute();
            return me.EmailAddress;
        }

    }
}
