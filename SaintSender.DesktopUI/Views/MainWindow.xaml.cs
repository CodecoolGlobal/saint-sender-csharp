
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GmailQuickstart;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using SaintSender.Core.Services;
using SaintSender.Core.Entities;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string NigerianPrinceName = "Oluwunmi Makinwa Akeem-omosanya";
        private UserCredential credential;
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            this._vm = new MainWindowViewModel();
            this.DataContext = _vm;
        }

        private void SignOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Gmail.RevokeToken(credential);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            Email email = (Email)item.Content;

            _vm.MarkAsRead(email.Id);
            email.Read = true;

            EmailWindow emailWindow = new EmailWindow();
            emailWindow.DataContext = email;
            Action showAction = () => emailWindow.Show();
            this.Dispatcher.BeginInvoke(showAction);
        }

        private void NewEmailButton_Click(object sender, RoutedEventArgs e)
        {
            NewEmailViewModel newEmailViewModel = new NewEmailViewModel(new EmailService());
            NewEmailWindow newEmailWindow = new NewEmailWindow(newEmailViewModel);
            newEmailWindow.Show();
        }
    }
}
