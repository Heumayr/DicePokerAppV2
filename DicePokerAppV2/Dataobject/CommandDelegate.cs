using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DicePokerAppV2.Dataobject
{
    public class CommandDelegate : ICommand
    {
        private readonly Predicate<object?>? _canExecute;
        private readonly Action<object?> _execute;

        public CommandDelegate(Predicate<object?>? CanExecute, Action<object?> Execute)
        {
            _canExecute = CanExecute;
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        }

        public bool CanExecute(object? parameter)
        {
            return (_canExecute == null || _canExecute(parameter));
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        
    }
}
