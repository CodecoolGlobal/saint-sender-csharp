using System.Collections.Generic;
using System.Windows;
using GmailQuickstart;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using SaintSender.Core.Services;
using SaintSender.Core.Entities;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string NigerianPrinceName = "Oluwunmi Makinwa Akeem-omosanya";
        private UserCredential credential;
        private GmailService gmail;
        private EmailService emailService;
        public MainWindow()
        {
            InitializeComponent();

            emailService = new EmailService();


            //UsersResource.LabelsResource.ListRequest request = Gmail.GetService().Users.Labels.List("me");
            //credential = Gmail.GetCredential();


        }

        private void GreetBtn_Click(object sender, RoutedEventArgs e)
        {
            var service = new GreetService();
            //var greeting = service.Greet(NigerianPrinceName);

            List<Email> emails = emailService.GetEmails();

            ResultTxt.Text = emails[3].ToString();

        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Gmail.RevokeToken(credential);
        }

    }
}
