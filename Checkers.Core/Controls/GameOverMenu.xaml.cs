using Checkers.Core.Models;
using Checkers.Core.Models.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Checkers.Core.Controls
{
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> SelectedOption;

        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();
            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.EndReason, gameState.CurrentPlayer);
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e) => SelectedOption?.Invoke(Option.Restart);

        private void Button_Exit_Click(object sender, RoutedEventArgs e) => SelectedOption?.Invoke(Option.Exit);

        private static string GetWinnerText(Player winner) => winner == Player.White ? "WHITE WINS!" : winner == Player.Red ? "RED WINS!" : "IT'S A DRAW";

        private static string GetPlayerString(Player player) => player == Player.White ? "RED" : player == Player.Red ? "WHITE" : "";

        private static string GetReasonText(EndReason endReason, Player currentPlayer)
        {
            switch (endReason)
            {
                case EndReason.Win: return $"{GetPlayerString(currentPlayer)} WINS!";
                case EndReason.FiftyMoveRule: return "FIFTY MOVE RULE";
                case EndReason.InsufficientMaterial: return "INSUFFICIENT MATERIAL";
                case EndReason.ThreefoldRepetition: return "THREEFOLD REPETITION";
                default: return "";
            }
        }
    }
}