using System;
using System.Windows.Input;

namespace Checkers.Core.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action execute, Predicate<object> canExecute = null) => (_execute, _canExecute) = (execute, canExecute);

        public event EventHandler CanExecuteChanged { add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value; }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute();
    }
}