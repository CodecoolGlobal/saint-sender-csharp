using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using SaintSender.Core.Services;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string NigerianPrinceName = "Oluwunmi Makinwa Akeem-omosanya";
        private UserCredential credential;
        public MainWindow()
        {
            InitializeComponent();

            UsersResource.LabelsResource.ListRequest request = Gmail.GetService().Users.Labels.List("kdani782@gmail.com");
            credential = Gmail.GetCredential();

        }

        private void GreetBtn_Click(object sender, RoutedEventArgs e)
        {
            var service = new GreetService();
            var greeting = service.Greet(NigerianPrinceName);
            ResultTxt.Text = greeting;

        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Gmail.RevokeToken(credential);
        }
    }
}
