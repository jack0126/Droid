using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jack.Mvvm.Droid
{
    public class CommandFactory
    {
        public static ICommand Action(Action action)
        {
            return new ActionCommand(action);
        }

        public static ICommand Action<T>(Action<T> action)
        {
            return new ActionCommand<T>(action);
        }
    }
    class ActionCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;
        public ActionCommand(Action execute) : this(execute, null)
        {
        }
        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute() : true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        public void Execute()
        {
            _execute?.Invoke();
        }
    }
    class ActionCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;
        public ActionCommand(Action<T> execute) : this(execute, null)
        {
        }

        public ActionCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute((T)parameter) : true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke((T)parameter);
        }
    }
}
