using ClassicGames.DAL;
using ClassicGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

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

        private void grdGames_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Oyunun kapak fotoğrafını güncellemek veya yeni bir oyun için yenisin eklemek istediğimizde devreye giren olay metodu
            
            // Win32 üstünden File Dialog kullanıyoruz
            var dialog = new OpenFileDialog
            {
                Title = "Choose Cover Image",
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

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetAllGames()
        {
            var games = _gameRepository.GetAll();
            grdGames.ItemsSource = games;
        }
    }
}
