using System.Collections.Generic;
using System.Windows;
using GmailQuickstart;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using SaintSender.Core.Services;
using SaintSender.Core.Entities;
using Thread = System.Threading.Thread;
using SaintSender.DesktopUI.ViewModels;

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
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            this._vm = new MainWindowViewModel();
            this.DataContext = _vm;
            


            //UsersResource.LabelsResource.ListRequest request = Gmail.GetService().Users.Labels.List("me");
            //credential = Gmail.GetCredential();


        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Gmail.RevokeToken(credential);
        }

    }
}
