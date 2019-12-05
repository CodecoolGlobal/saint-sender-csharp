using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaintSender.Core.Entities;

namespace SaintSender.Core.Services
{
    partial class EmailService
    {

        public static void BackupEmail(Email email)
        {
            string validDate = GenerateValidNames(email);

            string pathRoot = $@"..\..\..\SaintSender.Core\Resources\emailbackup";
            string targetPath = $@"{pathRoot}\{email.Subject}-{validDate}";

            CreateFoldersForEmail(email, targetPath);

            string fileName = $@"{targetPath}\{email.Id}.txt";

            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            StreamWriter writer = new StreamWriter(fileStream);

            writer.Write(email.Body);
            writer.Close();


            //File.WriteAllText(fileName, email.Body);

            string attachmentsPath = $@"{targetPath}\attachments";

            if (!Directory.Exists(attachmentsPath))
            {
                Directory.CreateDirectory(attachmentsPath);
            }

            int attCount = 0;
            foreach (byte[] attachment in email.Attachments)
            {
                try
                {
                    string path = $@"{attachmentsPath}\{attCount}";
                    File.WriteAllBytes(path, attachment);
                    attCount++;
                }
                catch (Exception)
                {
                    //stuff
                }
            }
        }
        private static string GenerateValidNames(Email email)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            string validDate = email.Date;

            foreach (char c in invalid)
            {
                validDate = validDate.Replace(c.ToString(), "");
            }

            return validDate;
        }

        private static void CreateFoldersForEmail(Email email, string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = $@"{targetPath}\{email.Id}.txt";

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }
    }
}
