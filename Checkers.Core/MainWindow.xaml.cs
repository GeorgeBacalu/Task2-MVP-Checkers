using System.Windows;

namespace Checkers.Core
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Button_NewGame_Click(object sender, RoutedEventArgs e) { new GameView().Show(); Close(); }
    }
}