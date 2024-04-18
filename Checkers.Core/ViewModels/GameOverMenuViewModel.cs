using Checkers.Core.Commands;
using Checkers.Core.Models;
using Checkers.Core.Models.Enums;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Checkers.Core.ViewModels
{
    internal class GameOverMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event Action<Option> SelectedOption;

        private string winnerText;
        public string WinnerText { get => winnerText; set { winnerText = value; OnPropertyChanged(nameof(WinnerText)); } }

        private string reasonText;
        public string ReasonText { get => reasonText; set { reasonText = value; OnPropertyChanged(nameof(ReasonText)); } }

        public ICommand RestartCommand { get; }
        public ICommand ExitCommand { get; }

        public GameOverMenuViewModel(GameState gameState)
        {
            Result result = gameState.Result;
            WinnerText = GetWinnerText(result.Winner);
            ReasonText = GetReasonText(result.EndReason, gameState.CurrentPlayer);
            RestartCommand = new RelayCommand(ExecuteRestart);
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        public void ExecuteRestart() => SelectedOption?.Invoke(Option.Restart);

        public void ExecuteExit() => SelectedOption?.Invoke(Option.Exit);

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