using System.Windows;
using System.Windows.Input;

namespace Aegis.Views
{
    public partial class ConfirmDeleteWindow : Window
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmDeleteWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
