using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace SaintSender.DesktopUI
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GmailService gmail = GmailQuickstart.Program.GetService();

        public void Test()
        {
            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = gmail.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }
    }
}
