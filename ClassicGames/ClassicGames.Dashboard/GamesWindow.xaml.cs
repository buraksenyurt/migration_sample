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
        IGameRepository _gameRepository;
        public GamesWindow(IGameRepository gameRepository)
        {
            // Repository'yi Constructor üzerinden enjete ediyoruz. Hangi nesnenin bağlanacağı Autofac yardımıyla App.xaml.cs içerisinde ayarlanmıştı
            _gameRepository = gameRepository;
            InitializeComponent();
            GetAllGames();
        }

        private void grdGames_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            // Bu olay tetiklendiyse grid üstünde ya güncelleme yapıp bir başka satıra geçmişizdir ya da en alttaki boş satır üstünden yeni bir veri içeriği girmişizdir
            var game = e.Row.DataContext as Game; // üzerinde çalıştığımız Game nesnesini bir yakalayalım

            if (game == null) // Null değilse tabii
                return;

            _gameRepository.UpsertGame(game); // Yeni bilgileri ile birlikte güncelleyelim
            GetAllGames(); // Güncel veriyi Grid'e çekelim
            MessageBox.Show($"{game.Name} güncellendi", "Add/Update Book", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        // Grid üstünden bir satır seçilip herhangibir tuşa basınca çalışan olay metodudur
        private void grdGames_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) // Delete tuşuna basılmış mı?
            {
                var grid = (DataGrid)sender; // Önce grid üstünde kaç satır seçili, seçili mi bulalım
                if (grid.SelectedItems.Count <= 0) // hiç satır seçilmediyse geri dön
                    return;

                if (grid.SelectedItems.Count > 1) // birden fazla oyunu silmeye müsaade edemeyiz
                {
                    MessageBox.Show("Tek seferde sadece bir oyun silinebilir biliyor muydun?", "Oyun Silme", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                var result = MessageBox.Show("Gerçekten bu oyunu veri tabanından silmek istiyor musun? Cidden mi?", "Oyun Silme", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    var game = grid.SelectedItem as Game; // seçili öğeyi Game nesnesine dönüştürelim
                    if (game != null && game.Id > 0) // Game nesnesi varsa 
                        _gameRepository.Delete(game.Id); // silelim
                }

                GetAllGames(); // tüm oyunları tekrar getirelim
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Oyunun kapak fotoğrafını güncellemek veya yeni bir oyun için yenisin eklemek istediğimizde devreye giren olay metodu

            // Win32 üstünden File Dialog kullanıyoruz
            var dialog = new OpenFileDialog
            {
                Title = "Oyunun kapak fotoğrafını seçelim",
                Filter = "JPEG (*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|PNG (*.png)|*.png|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                // Eğer var olan bir oyun üstünden buraya geldiysek güncelleme söz konusu olacaktır
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

        // Seçilen oyun bilgilerini JSON çıktı olarak almamızı sağlayan olay metodu
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // Eğer seçilin veri nesnesi Game tipinden değilse uyarı veriyorsa.
            // Aksi durumda onu game değişkeni olarak ele alabiliriz
            // Pattern Matching sağolsun
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
