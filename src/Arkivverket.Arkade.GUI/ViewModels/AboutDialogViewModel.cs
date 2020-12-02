using System;
using System.Diagnostics;
using Arkivverket.Arkade.Core.Util;
using Arkivverket.Arkade.GUI.Languages;
using Prism.Commands;
using Prism.Mvvm;

namespace Arkivverket.Arkade.GUI.ViewModels
{
    public class AboutDialogViewModel : BindableBase
    {
        public string VersionInfoString { get; }
        public string CopyrightInfoString { get; }
        public DelegateCommand ShowLicenseWebPageCommand { get; set; }
        public DelegateCommand ShowSiegfriedWebPageCommand { get; set; }
        public DelegateCommand ShowApacheLicenseWebPageCommand { get; set; }

        public AboutDialogViewModel()
        {
            VersionInfoString = AboutGUI.VersionText + ArkadeVersion.Current;
            CopyrightInfoString = string.Format(AboutGUI.ArkadeCopyrightInformationText, DateTime.Now.Year);

            ShowLicenseWebPageCommand = new DelegateCommand(ShowLicenseWebPage);
            ShowSiegfriedWebPageCommand = new DelegateCommand(ShowSiegfriedWebPage);
            ShowApacheLicenseWebPageCommand = new DelegateCommand(ShowApacheLicenseWebPage);
        }

        private void ShowLicenseWebPage()
        {
            Process.Start(AboutGUI.GnuGpl3_0Uri);
        }

        private void ShowSiegfriedWebPage()
        {
            Process.Start(AboutGUI.SiegfriedUri);
        }

        private void ShowApacheLicenseWebPage()
        {
            Process.Start(AboutGUI.ApacheV2_0Uri);
        }
    }
}
