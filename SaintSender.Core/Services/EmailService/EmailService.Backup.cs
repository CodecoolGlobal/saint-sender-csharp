﻿using System;
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

            string fileName = $@"{targetPath}\{email.Id}.txt";

            CreateFoldersForEmail(targetPath, fileName);

            SaveEmailText(fileName, email.Body);

            string attachmentsPath = $@"{targetPath}\attachments";

            if (email.Attachments.Count > 0)
            {
                CreateFolderForAttachments(attachmentsPath);
                SaveAttachments(attachmentsPath, email.Attachments);
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

        private static void CreateFoldersForEmail(string targetPath, string fileName)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        private static void SaveEmailText(string fileName, string emailBody)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.Write(emailBody);
            writer.Close();
            fileStream.Close();
        }

        private static void CreateFolderForAttachments(string attachmentsPath)
        {
            if (!Directory.Exists(attachmentsPath))
            {
                Directory.CreateDirectory(attachmentsPath);
            }
        }

        private static void SaveAttachments(string attachmentsPath, List<byte[]> attachments)
        {
            int attCount = 0;
            foreach (byte[] attachment in attachments)
            {
                try
                {
                    string path = $@"{attachmentsPath}\{attCount}.jpg";
                    File.WriteAllBytes(path, attachment);
                    attCount++;
                }
                catch (Exception)
                {
                    //stuff
                }
            }
        }
    }
}