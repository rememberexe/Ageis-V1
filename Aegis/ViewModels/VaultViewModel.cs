using Aegis.Commands;
using Aegis.Models;
using Aegis.Security;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using Aegis.Services.Persistence;
using System.Windows.Input;

namespace Aegis.ViewModels
{
    public class VaultViewModel : ViewModelBase
    {
        public ICommand SaveEntryCommand { get; }

        private readonly string _masterPassword;
        public RelayCommand SelectImageCommand { get; private set; }

        public ObservableCollection<VaultEntry> Entries { get; }

        private VaultEntry _selectedEntry;
        public VaultEntry SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                SelectImageCommand?.RaiseCanExecuteChanged();
            }

        }
        public void RefreshSelectedEntry()
        {
            OnPropertyChanged(nameof(SelectedEntry));
        }

        private void SelectImage()
        {
            

            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp",
                Title = "Select an image"
            };

            if (dialog.ShowDialog() == true)
            {
                // Resimleri app klasörüne kopyalayalım
                string imagesDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Aegis",
                    "Images");

                Directory.CreateDirectory(imagesDir);

                string destPath = Path.Combine(imagesDir, Path.GetFileName(dialog.FileName));
                File.Copy(dialog.FileName, destPath, true);

                SelectedEntry.ImagePath = destPath;
                SelectedEntry.Image = ImageLoader.LoadUnlocked(destPath);

                Save(); // 🔴 ÇOK ÖNEMLİ
                OnPropertyChanged(nameof(SelectedEntry));
            }
        }

        public RelayCommand AddEntryCommand { get; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        private bool CanSaveEntry()
        {
            return SelectedEntry != null;
        }
        private void SaveEntry()
        {
            if (SelectedEntry == null)
                return;
            var image = SelectedEntry.Image;
            SelectedEntry.Image = null;

            VaultPersistenceService.Save(Entries);

            SelectedEntry.Image = image;


            // RAM’e geri yükle
            SelectedEntry.Image = image;
        }


        public VaultViewModel(string masterPassword)
        {
            _masterPassword = masterPassword;

            Entries = new ObservableCollection<VaultEntry>(
                VaultService.LoadVault(masterPassword));

            foreach (var entry in Entries)
            {
                if (!string.IsNullOrWhiteSpace(entry.ImagePath) &&
                    File.Exists(entry.ImagePath))
                {
                    entry.Image = ImageLoader.LoadUnlocked(entry.ImagePath);
                }
            }

            AddEntryCommand = new RelayCommand(AddEntry);
            SaveCommand = new RelayCommand(Save, () => SelectedEntry != null);
            DeleteCommand = new RelayCommand(Delete, () => SelectedEntry != null);

            SelectImageCommand = new RelayCommand(
                SelectImage,
                () => SelectedEntry != null);
        }



        private void Save()
        {
            VaultService.SaveVault(_masterPassword, Entries.ToList());
        }


        private void Delete()
        {
            if (SelectedEntry == null)
                return;

            Entries.Remove(SelectedEntry);
            SelectedEntry = null;

            VaultService.SaveVault(_masterPassword, Entries.ToList());
        }

        public void AddEntry()
        {
            var entry = new VaultEntry
            {
                DisplayName = "New Entry",
                Username = string.Empty,
                Password = string.Empty,
                Notes = string.Empty,
                ImagePath = null
            };

            Entries.Add(entry);
            SelectedEntry = entry;
        }


    }
}
