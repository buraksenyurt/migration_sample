using ClassicGames.DAL;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        }

        private void grdGames_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {

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
