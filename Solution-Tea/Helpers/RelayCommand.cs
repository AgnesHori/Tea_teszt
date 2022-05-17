using System;
using System.Windows.Input;

namespace Solution_Tea.Helpers
{

    public class RelayCommand : ICommand
    {

        private readonly Action<object> execute;


        private readonly Predicate<object> canExecute;


        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }


        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute != null)
            {
                this.execute = execute;
            }
            else
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }


        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
