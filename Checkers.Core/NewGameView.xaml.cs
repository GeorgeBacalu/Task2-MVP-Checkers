using Checkers.Core.Controls;
using Checkers.Core.Models;
using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Moves;
using Checkers.Core.Models.Pieces;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers.Core.Views
{
    public partial class NewGameView : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly IDictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPosition = null;

        public NewGameView()
        {
            InitializeComponent();
            InitBoard();
            gameState = new GameState(Board.Init(), Player.Red);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
            UpdateCurrentPlayer();
            UpdatePieceCounts();
        }

        private void InitBoard()
        {
            for (int r = 0; r < pieceImages.GetLength(0); ++r)
                for (int c = 0; c < pieceImages.GetLength(1); ++c)
                {
                    Image image = new Image { Stretch = Stretch.Uniform, Margin = new Thickness(9) };
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);
                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < pieceImages.GetLength(0); ++r)
                for (int c = 0; c < pieceImages.GetLength(1); ++c)
                    pieceImages[r, c].Source = Images.Images.GetImage(board[r, c]);
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e) { new MainWindow().Show(); Close(); }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if( IsMenuVisible()) return;
            Position position = ToSquarePosition(e.GetPosition(BoardGrid));
            if (selectedPosition == null) OnFromSelectedPosition(position);
            else OnToSelectedPosition(position);
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            moves.ToList().ForEach(move => moveCache[move.To] = move);
        }

        private void ShowHighlights() => moveCache.Keys.ToList().ForEach(to => highlights[to.Row, to.Column].Fill = new SolidColorBrush(Color.FromArgb(100, 125, 255, 125)));

        private void ClearHighlights() => moveCache.Keys.ToList().ForEach(to => highlights[to.Row, to.Column].Fill = Brushes.Transparent);

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / highlights.GetLength(0);
            return new Position((int)(point.Y / squareSize), (int)(point.X / squareSize));
        }

        private void OnFromSelectedPosition(Position position)
        {
            IEnumerable<Move> moves = gameState.GetPieceLegalMoves(position);
            if (moves.Any())
            {
                selectedPosition = position;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToSelectedPosition(Position position)
        {
            selectedPosition = null;
            ClearHighlights();
            if (moveCache.TryGetValue(position, out Move move))
            {
                gameState.MakeMove(move);
                DrawBoard(gameState.Board);
                SetCursor(gameState.CurrentPlayer);
                if (gameState.IsGameOver()) ShowGameOver();
                UpdateCurrentPlayer();
                UpdatePieceCounts();
            }
        }

        private void SetCursor(Player player) => Cursor = player == Player.White ? Images.Images.whiteCursor : Images.Images.blackCursor;

        private void UpdateCurrentPlayer()
        {
            CurrentPlayer.Text = gameState.CurrentPlayer == Player.White ? "White's Turn" : "Red's Turn";
            CurrentPlayer.Foreground = gameState.CurrentPlayer == Player.White ? Brushes.White : Brushes.Red;
        }

        private void UpdatePieceCounts()
        {
            int whiteCount = 0, redCount = 0;
            for (int r = 0; r < pieceImages.GetLength(0); ++r)
                for (int c = 0; c < pieceImages.GetLength(1); ++c)
                {
                    Piece piece = gameState.Board[r, c];
                    if (piece != null)
                    {
                        if (piece.Color == Player.White) whiteCount++;
                        else redCount++;
                    }
                }
            WhiteCountText.Text = $"White Pieces: {whiteCount}";
            RedCountText.Text = $"Red Pieces: {redCount}";
        }

        private bool IsMenuVisible() => MenuContainer.Content != null;

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;
            gameOverMenu.SelectedOption += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    new MainWindow().Show();
                    Close();
                }
            };
        }

        private void RestartGame()
        {
            ClearHighlights();
            moveCache.Clear();
            gameState = new GameState(Board.Init(), Player.Red);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }
    }
}