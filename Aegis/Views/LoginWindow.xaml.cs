using Aegis.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Aegis.Models;

using System.Windows.Media.Animation;

namespace Aegis.Views
{
    public partial class LoginWindow : Window
    {
        private bool _passwordVisible;

        public LoginWindow()
        {
            InitializeComponent();
            Loaded += LoginWindow_Loaded;

            if (MasterPasswordService.MasterExists())
            {
                ModeText.Text = "Enter Master Password";
                ActionButton.Content = "Unlock";
            }
            else
            {
                ModeText.Text = "Create Master Password";
                ActionButton.Content = "Create Vault";
            }
        }
        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(160),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            var slideUp = new DoubleAnimation
            {
                From = 20,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            Storyboard sb = new Storyboard();

            sb.Children.Add(fadeIn);
            sb.Children.Add(slideUp);

            // ✅ DOĞRU TARGET
            Storyboard.SetTarget(fadeIn, LoginCard);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(Border.OpacityProperty));

            Storyboard.SetTarget(slideUp, CardTranslate);
            Storyboard.SetTargetProperty(slideUp, new PropertyPath(TranslateTransform.YProperty));

            sb.Begin();
        }


        // KARTTAN SÜRÜKLEME
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try { DragMove(); } catch { }
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_passwordVisible)
            {
                PasswordBox.Password = PasswordText.Text;
                PasswordText.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordText.Text = PasswordBox.Password;
                PasswordText.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
            }

            _passwordVisible = !_passwordVisible;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            string password = _passwordVisible
                ? PasswordText.Text
                : PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                StatusText.Text = "Password must be at least 6 characters.";
                StatusText.Visibility = Visibility.Visible;
                return;
            }

            // İLK KURULUM
            if (!MasterPasswordService.MasterExists())
            {
                MasterPasswordService.CreateMasterPassword(password);

                var vault = new VaultWindow(password);
                vault.Show();

                Close();
                return;
            }

            // YANLIŞ ŞİFRE
            if (!MasterPasswordService.VerifyMasterPassword(password))
            {
                ShakeCard();
                StatusText.Text = "Invalid master password.";
                StatusText.Visibility = Visibility.Visible;
                return;
            }

            // ✅ DOĞRU ŞİFRE → VAULT AÇ
            var vaultWindow = new VaultWindow(password);
            vaultWindow.Show();

            Close();
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(140),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseIn
                }
            };

            var slideDown = new DoubleAnimation
            {
                To = 20,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseIn
                }
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(fadeOut);
            sb.Children.Add(slideDown);

            Storyboard.SetTarget(fadeOut, LoginCard);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(Border.OpacityProperty));

            Storyboard.SetTarget(slideDown, CardTranslate);
            Storyboard.SetTargetProperty(slideDown, new PropertyPath(TranslateTransform.YProperty));

            sb.Completed += (_, _) => Close();
            sb.Begin();
        }
        private void ShakeCard()
        {
            var shake = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(320)
            };

            shake.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            shake.KeyFrames.Add(new EasingDoubleKeyFrame(-10, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(60))));
            shake.KeyFrames.Add(new EasingDoubleKeyFrame(10, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(120))));
            shake.KeyFrames.Add(new EasingDoubleKeyFrame(-8, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(180))));
            shake.KeyFrames.Add(new EasingDoubleKeyFrame(8, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(240))));
            shake.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(320))));

            Storyboard sb = new Storyboard();
            sb.Children.Add(shake);

            Storyboard.SetTarget(shake, CardTranslate);
            Storyboard.SetTargetProperty(shake, new PropertyPath(TranslateTransform.XProperty));

            sb.Begin();
        }


    }
}
