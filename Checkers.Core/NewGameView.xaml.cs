using Checkers.Core.Models;
using Checkers.Core.Models.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checkers.Core.Views
{
    public partial class NewGameView : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private GameState gameState;

        public NewGameView()
        {
            InitializeComponent();
            InitBoard();
            gameState = new GameState(Board.Init(), Player.White);
            DrawBoard(gameState.Board);
        }

        private void InitBoard()
        {
            for (int r = 0; r < pieceImages.GetLength(0); ++r)
                for (int c = 0; c < pieceImages.GetLength(1); ++c)
                {
                    var image = new Image { Stretch = Stretch.Uniform, Margin = new Thickness(9) };
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);
                }
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < pieceImages.GetLength(0); ++r)
                for (int c = 0; c < pieceImages.GetLength(1); ++c)
                    pieceImages[r, c].Source = Images.Images.GetImage(board[r, c]);
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e) { new MainWindow().Show(); Close(); }
    }
}