using Aegis.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aegis.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Auto-lock
            foreach (ComboBoxItem item in AutoLockCombo.Items)
            {
                if ((string)item.Tag == SettingsService.Current.AutoLockMinutes.ToString())
                {
                    AutoLockCombo.SelectedItem = item;
                    break;
                }
            }

            ClipboardCheck.IsChecked = SettingsService.Current.ClearClipboard;

        }

        private void AutoLock_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (AutoLockCombo.SelectedItem is ComboBoxItem item)
            {
                SettingsService.Current.AutoLockMinutes =
                    int.Parse(item.Tag.ToString());

                SettingsService.Save();
            }
        }

        private void Clipboard_Checked(object sender, RoutedEventArgs e)
        {
            SettingsService.Current.ClearClipboard = true;
            SettingsService.Save();
        }

        private void Clipboard_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsService.Current.ClearClipboard = false;
            SettingsService.Save();
        }

        private void Theme_Changed(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void ApplyTheme()
        {
            if (SettingsService.Current.Theme == "TrueBlack")
                Application.Current.Resources["WindowBackground"] = "#000000";
            else
                Application.Current.Resources["WindowBackground"] = "#0F0F0F";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
