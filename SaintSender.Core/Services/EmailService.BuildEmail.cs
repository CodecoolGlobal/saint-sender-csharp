using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Gmail.v1.Data;
using Org.BouncyCastle.Utilities.Encoders;
using SaintSender.Core.Entities;

namespace SaintSender.Core.Services
{
    partial class EmailService
    {
        private Email BuildEmail(Message message, string messageId)
        {
            IList<MessagePartHeader> messageHeaders = message.Payload.Headers;

            string from = "";
            string to = "";
            string subject = "";
            string date = "";

            GetHeaders(ref from, ref to, ref subject, ref date, messageHeaders);

            IList<MessagePart> messageParts = message.Payload.Parts;

            StringBuilder bodyBuilder = new StringBuilder(); // lul
            GetBodyFromNestedParts(messageId, messageParts, bodyBuilder);

            if (bodyBuilder.Length == 0)
            {
                bodyBuilder.Append(message.Payload.Body.Data);
            }

            string plainBody = DecodeMessageBody(bodyBuilder);

            return new Email(from, to, date, subject, plainBody);
        }

        private void GetHeaders(ref string from, ref string to, ref string subject, ref string date, IList<MessagePartHeader> messageHeaders)
        {
            foreach (MessagePartHeader header in messageHeaders)
            {

                if (header.Name == "From")
                {
                    from = header.Value;
                }

                else if (header.Name == "To")
                {
                    to = header.Value;
                }

                else if (header.Name == "Subject")
                {
                    subject = header.Value;
                }

                else if (header.Name == "Date")
                {
                    date = header.Value;
                }
            }
        }

        private void GetBodyFromNestedParts(string messageId, IList<MessagePart> messageParts, StringBuilder stringBuilder)
        {
            if (messageParts != null)
            {
                foreach (MessagePart messagePart in messageParts)
                {

                    SaveAttachments(messagePart, messageId);

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

        private string DecodeMessageBody(StringBuilder bodyBuilder)
        {
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

            return Encoding.UTF8.GetString(bodyBytes);
        }
    }
}
