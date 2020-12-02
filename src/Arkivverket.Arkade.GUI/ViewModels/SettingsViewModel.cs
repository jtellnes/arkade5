using System;
using System.Threading;
using Arkivverket.Arkade.Core.Base;
using System.Windows;
using System.Windows.Forms;
using Arkivverket.Arkade.Core.Languages;
using Arkivverket.Arkade.GUI.Properties;
using Arkivverket.Arkade.GUI.Util;
using Arkivverket.Arkade.GUI.Languages;
using Prism.Commands;
using Prism.Mvvm;
using Serilog;
using MessageBox = System.Windows.MessageBox;

namespace Arkivverket.Arkade.GUI.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly ILogger _log = Log.ForContext<SettingsViewModel>();
        private string _arkadeProcessingAreaLocationSetting;
        private bool _darkModeSelected;
        private SupportedLanguages? _selectedUILanguage;
        private SupportedLanguages? _selectedOutputLanguage;

        public string CurrentArkadeProcessingAreaLocation { get; }
        public string DirectoryNameArkadeProcessingAreaRoot { get; }

        public bool DarkModeSelected
        {
            get => _darkModeSelected;
            set => SetProperty(ref _darkModeSelected, value);
        }

        public SupportedLanguages? SelectedUILanguage
        {
            get => _selectedUILanguage;
            set => SetProperty(ref _selectedUILanguage, value);
        }
        
        public SupportedLanguages? SelectedOutputLanguage
        {
            get => _selectedOutputLanguage;
            set => SetProperty(ref _selectedOutputLanguage, value);
        }

        public string ArkadeProcessingAreaLocationSetting
        {
            get => _arkadeProcessingAreaLocationSetting;
            set => SetProperty(ref _arkadeProcessingAreaLocationSetting, value);
        }

        public DelegateCommand ChangeArkadeProcessingAreaLocationCommand { get; }
        public DelegateCommand ApplyChangesCommand { get; }

        public SettingsViewModel()
        {
            ArkadeProcessingAreaLocationSetting = Util.ArkadeProcessingAreaLocationSetting.Get();
            CurrentArkadeProcessingAreaLocation = ArkadeProcessingArea.Location?.FullName;
            DirectoryNameArkadeProcessingAreaRoot = Core.Util.ArkadeConstants.DirectoryNameArkadeProcessingAreaRoot;

            ChangeArkadeProcessingAreaLocationCommand = new DelegateCommand(ChangeArkadeProcessingAreaLocation);
            ApplyChangesCommand = new DelegateCommand(ApplyChanges);

            DarkModeSelected = Settings.Default.DarkModeEnabled;
            if (!LanguageManager.TryParseFromString(Settings.Default.SelectedOutputLanguage, out _selectedOutputLanguage))
                Settings.Default.SelectedOutputLanguage = Thread.CurrentThread.CurrentCulture.ToString();
            if (!LanguageManager.TryParseFromString(Settings.Default.SelectedOutputLanguage, out _selectedUILanguage))
                Settings.Default.SelectedUILanguage = Thread.CurrentThread.CurrentCulture.ToString();

        }

        private void ChangeArkadeProcessingAreaLocation()
        {
            if (!ArkadeInstance.IsOnlyInstance)
            {
                string message = SettingsGUI.OtherInstancesRunningOnProcessingAreaChangeMessage;
                MessageBox.Show(message, "NB!", MessageBoxButton.OK, MessageBoxImage.Error);

                _log.Information("Arkade processing area location change denied due to other running Arkade instances");

                return;
            }

            _log.Information("User action: Open choose Arkade processing area location dialog");

            var selectDirectoryDialog = new FolderBrowserDialog();

            if (selectDirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                ArkadeProcessingAreaLocationSetting = selectDirectoryDialog.SelectedPath;

                _log.Information(
                    "User action: Choose Arkade processing area location {ArkadeDirectoryLocationSetting}",
                    ArkadeProcessingAreaLocationSetting
                );
            }
            else _log.Information("User action: Abort choose Arkade processing area location");
        }

        private void ApplyChanges()
        {
            ApplySelectedMode();
            ApplyUILanguageSelection();
            ApplyOutputLanguageSelection();
            Util.ArkadeProcessingAreaLocationSetting.Set(ArkadeProcessingAreaLocationSetting);
        }

        private void ApplySelectedMode()
        {
            Settings.Default.DarkModeEnabled = DarkModeSelected;

            if (DarkModeSelected)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }
        }

        public static void ApplyDarkMode()
        {
            App.Current.Resources.MergedDictionaries[0].Source = new Uri(
                @"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/materialdesigntheme.dark.xaml");
        }
        
        public static void ApplyLightMode()
        {
            App.Current.Resources.MergedDictionaries[0].Source = new Uri(
                @"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/materialdesigntheme.light.xaml");
        }

        private void ApplyUILanguageSelection()
        {
            string currentCulture = Settings.Default.SelectedUILanguage;

            Settings.Default.SelectedUILanguage = SelectedUILanguage switch
            {
                SupportedLanguages.English => "en-GB",
                SupportedLanguages.Norsk_BM => "nb-NO",
                _ => Settings.Default.SelectedUILanguage
            };

            MainWindowViewModel.UiLanguageIsChanged = currentCulture != Settings.Default.SelectedUILanguage;

            Settings.Default.Save();
        }

        private void ApplyOutputLanguageSelection()
        {
            Settings.Default.SelectedOutputLanguage = SelectedOutputLanguage switch
            {
                SupportedLanguages.English => "en-GB",
                SupportedLanguages.Norsk_BM => "nb-NO",
                _ => Settings.Default.SelectedOutputLanguage
            };

            Settings.Default.Save();
        }
    }
}
