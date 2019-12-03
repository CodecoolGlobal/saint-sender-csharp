using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GmailQuickstart;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Org.BouncyCastle.Utilities.Encoders;
using SaintSender.Core.Entities;

namespace SaintSender.Core.Services
{
    public class EmailService
    {
        private GmailService gmail;

        public EmailService()
        {
            gmail = Gmail.GetService();
        }

        public List<Email> GetEmails()
        {
            List<Email> emails = new List<Email>();

            UsersResource.MessagesResource.ListRequest messageRequest = gmail.Users.Messages.List("me");

            messageRequest.MaxResults = 20;
            messageRequest.IncludeSpamTrash = true;
            messageRequest.LabelIds = "INBOX";

            ListMessagesResponse messageResponse = messageRequest.Execute();

            foreach (Message message in messageResponse.Messages)
            {
                Message messageInfo = gmail.Users.Messages.Get("me", message.Id).Execute();

                Email email = BuildEmail(messageInfo, message.Id);
                emails.Add(email);
            }

            return emails;
        }

        private Email BuildEmail(Message message, string messageId)
        {
            IList<MessagePartHeader> messageHeaders = message.Payload.Headers;

            string from = "";
            string to = "";
            string subject = "";
            string date = "";

            //GetHeaders(out from, out to, out subject, out date, messageHeaders);

            foreach (MessagePartHeader header in messageHeaders)
            {
                //16
                if (header.Name == "From")
                {
                    from = header.Value;
                }
                //20
                else if (header.Name == "To")
                {
                    to = header.Value;
                }
                //19
                else if (header.Name == "Subject")
                {
                    subject = header.Value;
                }
                //17
                else if (header.Name == "Date")
                {
                    date = header.Value;
                }
            }


            IList<MessagePart> messageParts = message.Payload.Parts;

            StringBuilder bodyBuilder = new StringBuilder(); // lul
            GetBodyFromNestedParts(messageId, messageParts, bodyBuilder); // message id here

            if (bodyBuilder.Length == 0)
            {
                bodyBuilder.Append(message.Payload.Body.Data);
            }

            string sbContent = bodyBuilder.ToString();

            byte[] bodyBytes;

            try
            {
                bodyBytes = Base64.Decode(sbContent);
            }
            catch (System.FormatException)
            {
                sbContent = bodyBuilder.ToString().Replace("-", "+").Replace("_", "/");
                bodyBytes = Base64.Decode(sbContent);
            }

            string plainBody = Encoding.UTF8.GetString(bodyBytes);

            return new Email(from, to, date, subject, plainBody);

        }

        //private void GetHeaders(out string from, out string to, out string subject, out string date, IList<MessagePartHeader> messageHeaders)
        //{
        //    foreach (MessagePartHeader header in messageHeaders)
        //    {

        //        if (header.Name == "From")
        //        {
        //            from = header.Value;
        //        }

        //        else if (header.Name == "To")
        //        {
        //            to = header.Value;
        //        }

        //        else if (header.Name == "Subject")
        //        {
        //            subject = header.Value;
        //        }

        //        else if (header.Name == "Date")
        //        {
        //            date = header.Value;
        //        }
        //    }
        //}

        private void GetBodyFromNestedParts(string messageId, IList<MessagePart> messageParts, StringBuilder stringBuilder)
        {

            if (messageParts != null)
            {
                foreach (MessagePart messagePart in messageParts)
                {
                    if (!string.IsNullOrEmpty(messagePart.Filename))
                    {
                        string attId = messagePart.Body.AttachmentId;
                        MessagePartBody attachPart = gmail.Users.Messages.Attachments.Get("me", messageId, attId).Execute();

                        // Converting from RFC 4648 base64 to base64url encoding
                        // see http://en.wikipedia.org/wiki/Base64#Implementations_and_history
                        string attachData = attachPart.Data.Replace('-', '+');
                        attachData = attachData.Replace('_', '/');

                        byte[] data = Convert.FromBase64String(attachData);
                        File.WriteAllBytes(Path.Combine(@"C:\Users\Dániel Kalocsay\Desktop\emailbackup", messagePart.Filename), data);
                    }

                    if (messagePart.MimeType == "text/plain")
                    {
                        stringBuilder.Append(messagePart.Body.Data);
                    }

                    if (messagePart.Parts != null)
                    {
                        GetBodyFromNestedParts(messageId, messagePart.Parts, stringBuilder);
                    }
                }

            }

        }
    }
}
