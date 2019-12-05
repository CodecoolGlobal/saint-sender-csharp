using SaintSender.Core.Entities;
using SaintSender.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainWindowViewModel
    {
        private EmailService _emailService;
        public ObservableCollection<Email> Emails { get; private set; }
        public object LockEmails { get; set; }
        public long TimeStamp { get; set; }

        public MainWindowViewModel()
        {
            this._emailService = new EmailService();
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            LockEmails = new object();

            Emails = new ObservableCollection<Email>(_emailService.GetEmails(false, TimeStamp));

            BindingOperations.EnableCollectionSynchronization(Emails,LockEmails);
            Timer();
        }

        private void Timer()
        {
            var timer = new System.Timers.Timer(5000);
            timer.Elapsed += Sync;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            
        }

        private void Sync(object sender, System.Timers.ElapsedEventArgs e)
        {
            var emails = _emailService.GetEmails(true, TimeStamp);
            lock (LockEmails)
            {

                foreach (var email in emails)
                {
                    Emails.Insert(0,email);
                    
                }
            }
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
