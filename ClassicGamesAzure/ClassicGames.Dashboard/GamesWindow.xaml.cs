using ClassicGames.DAL;
using ClassicGames.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClassicGames.Dashboard
{
    /// <summary>
    /// Interaction logic for GamesWindow.xaml
    /// </summary>
    public partial class GamesWindow : Window
    {
        readonly IGameRepository _gameRepository;
        public GamesWindow(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            InitializeComponent();
            GetAllGames();
        }

        private void grdGames_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var game = e.Row.DataContext as Game; 

            if (game == null)
                return;

            _gameRepository.UpsertGame(game);
            GetAllGames(); 
            MessageBox.Show($"{game.Name} güncellendi", "Add/Update Book", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void grdGames_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;
                if (grid.SelectedItems.Count <= 0) 
                    return;

                if (grid.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Tek seferde sadece bir oyun silinebilir biliyor muydun?", "Oyun Silme", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                var result = MessageBox.Show("Gerçekten bu oyunu veri tabanından silmek istiyor musun? Cidden mi?", "Oyun Silme", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    var game = grid.SelectedItem as Game; 
                    if (game != null && game.Id > 0)
                        _gameRepository.Delete(game.Id); 
                }

                GetAllGames(); 
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Oyunun kapak fotoğrafını seçelim",
                Filter = "JPEG (*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|PNG (*.png)|*.png|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                if (((FrameworkElement)sender).DataContext is Game game && game.Id > 0)
                {
                    game.Photo = GetPhoto(dialog.FileName);
                    _gameRepository.UpsertGame(game);

                    GetAllGames();
                    MessageBox.Show("Kapak fotoğrafı yüklendi!", "Kapak Fotoğrafı", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Fotoğrafı yükleyemedim yaa. Sanırım önce bir oyun girmen lazım. :(", "Oyun Ekle", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private byte[] GetPhoto(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    byte[] photo = reader.ReadBytes((int)stream.Length);
                    return photo;
                }
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (!(((FrameworkElement)sender).DataContext is Game game))
            {
                MessageBox.Show("JSON çıktısı almak için bir oyun seçilmeli", "JSON Çıktı", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Title = "Bilgileri JSON Olarak Çıkartalım",
                Filter = "JSON (*.json)|*.json|All Files (*.*)|*.*",
                FileName = game.Name
            };
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                using (StreamWriter file = File.CreateText(dialog.FileName))
                {
                    var serializedJson = JsonConvert.SerializeObject(game, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        });

                    file.Write(serializedJson);
                    file.Close();
                }

                MessageBox.Show("JSON çıktı işlemi başarılı!", "Book Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GetAllGames()
        {
            var games = _gameRepository.GetAll();
            grdGames.ItemsSource = games;
        }
    }
}
