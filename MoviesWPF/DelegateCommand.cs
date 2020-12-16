using System;
using System.Windows.Input;

namespace MoviesWPF
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        public DelegateCommand(Predicate<object> predicate, Action<object> action) 
        {
            _canExecute = predicate;
            _execute = action;
        }
        public DelegateCommand(Action<object> action) 
        {
            _execute = action;
            _canExecute = x => false;
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
