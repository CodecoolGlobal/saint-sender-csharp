using System;
using System.IO;
using Google.Apis.Gmail.v1.Data;

namespace SaintSender.Core.Services
{
    public partial class EmailService
    {
        private void SaveAttachments(MessagePart messagePart, string messageId)
        {
            string attId = messagePart.Body.AttachmentId;
            MessagePartBody attachPart = _gmail.Users.Messages.Attachments.Get("me", messageId, attId).Execute();

            // Converting from RFC 4648 base64 to base64url encoding
            // see http://en.wikipedia.org/wiki/Base64#Implementations_and_history
            string attachData = attachPart.Data.Replace('-', '+');
            attachData = attachData.Replace('_', '/');

            byte[] data = Convert.FromBase64String(attachData);

            string targetPath = $@"..\..\..\SaintSender.Core\Resources\emailbackup\{messageId}";

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                File.WriteAllBytes(Path.Combine(targetPath, messagePart.Filename), data);
            }

            File.WriteAllBytes(Path.Combine(targetPath, messagePart.Filename), data);
        }

    }
}
