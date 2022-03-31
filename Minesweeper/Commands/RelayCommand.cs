using System;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private event Action<object?> _execute;
        private event Predicate<object?> _canExecute;


        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter)
            => _execute.Invoke(parameter);
    }
}
