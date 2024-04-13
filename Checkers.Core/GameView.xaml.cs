using Checkers.Core.Models.Moves;
using Checkers.Core.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using Checkers.Core.Models.Enums;
using Checkers.Core.Controls;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using Checkers.Core.Data;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace Checkers.Core
{
    public partial class GameView : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly IDictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private readonly List<Move> gameMoves = new List<Move>();
        private readonly DataManager gameDataManager = new DataManager("../../Data/game.json");
        private readonly string ScorePath = "../../Data/score.txt";
        private GameState gameState;
        private Position selectedPosition = null;
        private int whiteScore, redScore;

        public GameView(List<Move> moves = null)
        {
            InitializeComponent();
            InitBoard();
            gameState = new GameState(Board.Init(), Player.Red);
            MakeMoves(moves);
            (whiteScore, redScore) = ReadScoreFromFile();
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
            UpdateScoreDisplay();
            UpdatePieceCountsDisplay();
            UpdateCurrentPlayerDisplay();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e) { new MainWindow().Show(); Close(); }

        private void Button_SaveGame_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "game" };
            if (dialog.ShowDialog() == true)
                File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(gameMoves, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
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
                    pieceImages[r, c].Source = Assets.Assets.GetImage(board[r, c]);
        }

        private void MakeMoves(List<Move> moves)
        {
            moves?.ForEach(move => { gameState.MakeMove(move); gameMoves.Add(move); });
            gameDataManager.SaveData(gameMoves);
        }

        private void SetCursor(Player player) => Cursor = player == Player.White ? Assets.Assets.whiteCursor : Assets.Assets.blackCursor;

        private void UpdateScoreDisplay() => Score.Text = $"White: {whiteScore} Red: {redScore}";

        private void UpdatePieceCountsDisplay()
        {
            int whiteCount = gameState.Board.GetPlayerPieceCount(Player.White), redCount = gameState.Board.GetPlayerPieceCount(Player.Red);
            WhiteCountText.Text = $"White Pieces: {whiteCount}";
            RedCountText.Text = $"Red Pieces: {redCount}";
        }

        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayer.Text = gameState.CurrentPlayer == Player.White ? "White's Turn" : "Red's Turn";
            CurrentPlayer.Foreground = gameState.CurrentPlayer == Player.White ? Brushes.White : Brushes.Red;
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuVisible()) return;
            Position position = ToSquarePosition(e.GetPosition(BoardGrid));
            if (selectedPosition == null) OnFromSelectedPosition(position);
            else OnToSelectedPosition(position);
        }

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
                gameMoves.Add(move);
                gameDataManager.SaveData(gameMoves);
                DrawBoard(gameState.Board);
                SetCursor(gameState.CurrentPlayer);
                if (gameState.IsGameOver())
                {
                    if (gameState.Result.Winner == Player.White) ++whiteScore;
                    else if (gameState.Result.Winner == Player.Red) ++redScore;
                    UpdateScoreDisplay();
                    WriteScoreToFile(whiteScore, redScore);
                    ShowGameOver();
                }
                UpdatePieceCountsDisplay();
                UpdateCurrentPlayerDisplay();
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            moves.ToList().ForEach(move => moveCache[move.To] = move);
        }

        private void ShowHighlights() => moveCache.Keys.ToList().ForEach(to => highlights[to.Row, to.Column].Fill = new SolidColorBrush(Color.FromArgb(100, 125, 255, 125)));

        private void ClearHighlights() => moveCache.Keys.ToList().ForEach(to => highlights[to.Row, to.Column].Fill = Brushes.Transparent);

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
                    ClearHighlights();
                    moveCache.Clear();
                    gameState = new GameState(Board.Init(), Player.Red);
                    DrawBoard(gameState.Board);
                    SetCursor(gameState.CurrentPlayer);
                    UpdateScoreDisplay();
                }
                else
                {
                    new MainWindow().Show(); Close();
                    (whiteScore, redScore) = (0, 0);
                    WriteScoreToFile(whiteScore, redScore);
                }
            };
        }

        private (int whiteScore, int redScore) ReadScoreFromFile()
        {
            if (!File.Exists(ScorePath)) return (0, 0);
            string[] lines = File.ReadAllLines(ScorePath);
            int whiteScore = int.Parse(lines[0].Split(':')[1].Trim()), redScore = int.Parse(lines[1].Split(':')[1].Trim());
            return (whiteScore, redScore);
        }

        private void WriteScoreToFile(int whiteScore, int redScore) => File.WriteAllLines(ScorePath, new string[] { $"White: {whiteScore}", $"Red: {redScore}" });
    }
}