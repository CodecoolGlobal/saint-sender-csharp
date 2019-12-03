using SaintSender.Core.Entities;
using SaintSender.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.DesktopUI.ViewModels
{
    class MainWindowViewModel
    {
        private EmailService _emailService;
        public ObservableCollection<Email> Emails { get; private set; }

        public MainWindowViewModel()
        {
            this._emailService = new EmailService();
            Emails = new ObservableCollection<Email>(_emailService.GetEmails());
        }
    }
}
