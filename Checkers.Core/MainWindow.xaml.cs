using Checkers.Core.Data;
using Checkers.Core.Models.Moves;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Checkers.Core
{
    public partial class MainWindow : Window
    {
        private DataManager gameDataManager;
        private bool allowMultipleJumps = false;

        public MainWindow() => InitializeComponent();

        private void Button_NewGame_Click(object sender, RoutedEventArgs e) { new GameView(allowMultipleJumps).Show(); Close(); }

        private void Button_OpenGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "game" };
            if (dialog.ShowDialog() == true)
            {
                gameDataManager = new DataManager(dialog.FileName);
                List<Move> moves = gameDataManager.LoadData<List<Move>>();
                new GameView(allowMultipleJumps, moves).Show(); Close();
            }
        }

        private void Button_AllowMultipleJumps_Click(object sender, RoutedEventArgs e)
        {
            allowMultipleJumps = !allowMultipleJumps;
            MultipleJumpsModeButton.Content = allowMultipleJumps ? "Disable Multiple Jumps" : "Enable Multiple Jumps";
        }
    }
}