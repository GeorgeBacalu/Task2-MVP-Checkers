using Checkers.Core.Commands;
using Checkers.Core.Data;
using Checkers.Core.Models.Moves;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Checkers.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event Action OnClose;

        private DataManager gameDataManager;

        private bool allowMultipleJumps = false;
        public bool AllowMultipleJumps { get => allowMultipleJumps; set { allowMultipleJumps = value; OnPropertyChanged(nameof(AllowMultipleJumpsText)); } }

        public string AllowMultipleJumpsText => allowMultipleJumps ? "Disable Multiple Jumps" : "Enable Multiple Jumps";

        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand ToggleJumpsModeCommand { get; }
        public ICommand ShowStatisticsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainViewModel()
        {
            NewGameCommand = new RelayCommand(ExecuteNewGame);
            OpenGameCommand = new RelayCommand(ExecuteOpenGame);
            ToggleJumpsModeCommand = new RelayCommand(ExecuteToggleJumpsMode);
            ShowStatisticsCommand = new RelayCommand(ExecuteShowStatistics);
            ShowAboutCommand = new RelayCommand(ExecuteShowAbout);
        }

        private void ExecuteNewGame() => new GameView(AllowMultipleJumps).Show();

        private void ExecuteOpenGame()
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "game" };
            if (dialog.ShowDialog() == true)
            {
                gameDataManager = new DataManager(dialog.FileName);
                List<Move> moves = gameDataManager.LoadData<List<Move>>();
                new GameView(AllowMultipleJumps, moves).Show();
            }
        }

        private void ExecuteToggleJumpsMode() => AllowMultipleJumps = !AllowMultipleJumps;

        private void ExecuteShowStatistics() => MessageBox.Show(File.ReadAllText("../../Data/statistics.txt"));

        private void ExecuteShowAbout() => MessageBox.Show(@"
            Proiect dezvoltat de George Bacalu
            Adresa institutionala: george.bacalu@student.unitbv.ro
            Grupa: 10LF321
            Descriere: Joc de dame");
    }
}