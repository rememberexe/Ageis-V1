using Aegis.Services;
using Aegis.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Aegis.Views
{
    public partial class VaultWindow : Window
    {
        private readonly DispatcherTimer _autoLockTimer;
        private TimeSpan _remainingTime;
        private TimeSpan AutoLockDuration =>
            TimeSpan.FromMinutes(SettingsService.Current.AutoLockMinutes);
        public string ImagePath { get; set; }
        public BitmapImage Image { get; set; }

        public VaultWindow(string masterPassword)
        {
            InitializeComponent();
            DataContext = new VaultViewModel(masterPassword);
            _remainingTime = AutoLockDuration;

            _autoLockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _autoLockTimer.Tick += AutoLockTimer_Tick;
            _autoLockTimer.Start();

            UpdateAutoLockText();
        } // 🔁 Her kullanıcı aktivitesinde çağrılır
        private void Window_UserActivity(object sender, InputEventArgs e)
        {
            ResetAutoLock();
        }

        private void ResetAutoLock()
        {
            _remainingTime = AutoLockDuration;
            UpdateAutoLockText();
        }

        private void AutoLockTimer_Tick(object? sender, EventArgs e)
        {
            _remainingTime -= TimeSpan.FromSeconds(1);

            if (_remainingTime <= TimeSpan.Zero)
            {
                TriggerAutoLock();
                return;
            }

            UpdateAutoLockText();
        }

        private void UpdateAutoLockText()
        {
            AutoLockText.Text =
                $"Auto-lock in {_remainingTime:mm\\:ss}";
        }

        private void TriggerAutoLock()
        {
            _autoLockTimer.Stop();

            // Vault'u gizle
            this.Hide();

            // Login ekranını aç
            var login = new LoginWindow();
            login.ShowDialog();

           
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var confirm = new ConfirmDeleteWindow
            {
                Owner = this
            };

            confirm.ShowDialog();

            if (confirm.IsConfirmed)
            {
                if (DataContext is VaultViewModel vm)
                {
                    vm.DeleteCommand.Execute(null);
                }
            }
        }


        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not VaultViewModel vm || vm.SelectedEntry == null)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp",
                Title = "Select image"
            };

            if (dialog.ShowDialog() != true)
                return;

            string imagesDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Aegis",
                "Images");

            Directory.CreateDirectory(imagesDir);

            string fileName = Guid.NewGuid() + Path.GetExtension(dialog.FileName);
            string destPath = Path.Combine(imagesDir, fileName);

            File.Copy(dialog.FileName, destPath, true);

            // 🔴 ÖNEMLİ: önce eski referansı kopar
            vm.SelectedEntry.Image = null;

            // ✅ Dosyayı kilitlemeden yükle
            var bitmap = ImageLoader.LoadUnlocked(destPath);

            vm.SelectedEntry.ImagePath = destPath;
            vm.SelectedEntry.Image = bitmap;

            vm.RefreshSelectedEntry();
            //vm.SaveEntryCommand.Execute(null);

        }

        private void SetIcon_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VaultViewModel vm &&
                vm.SelectedEntry != null &&
                sender is Button btn &&
                btn.Tag is string icon)
            {
                vm.SelectedEntry.ImagePath =
                    $"pack://application:,,,/Assets/Icons/{icon}.png";
            }
        }


        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow
            {
                Owner = this
            };
            settings.ShowDialog();
        }

    }
}
