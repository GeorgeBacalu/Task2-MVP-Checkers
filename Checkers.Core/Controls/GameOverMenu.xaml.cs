using Checkers.Core.Models;
using Checkers.Core.Models.Enums;
using Checkers.Core.ViewModels;
using System;
using System.Windows.Controls;

namespace Checkers.Core.Controls
{
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> SelectedOption;

        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();
            var viewModel = new GameOverMenuViewModel(gameState);
            viewModel.SelectedOption += (option) => SelectedOption?.Invoke(option);
            DataContext = viewModel;
        }
    }
}