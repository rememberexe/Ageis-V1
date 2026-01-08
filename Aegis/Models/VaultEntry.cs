using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Text.Json.Serialization;

namespace Aegis.Models
{
    public class VaultEntry : INotifyPropertyChanged
    {
        private string _title;
        private string _username;
        private string _password;
        private string _notes;
        private string _imagePath;
        private string _displayName;
        private BitmapImage _image;

        // Kart / banka bilgileri
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; } // MM/YY
        public string CVV { get; set; }
        public string BankName { get; set; }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Disk üzerindeki dosya yolu (sadece saklama amaçlı)
        /// UI ASLA buna bind edilmez
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// UI’nin bind edildiği, RAM’de tutulan image
        /// Dosya kilidi YOK
        /// </summary>
        [JsonIgnore]
        public BitmapImage Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
