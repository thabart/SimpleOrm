using System;
using System.Windows.Input;

namespace ORM.DisplayGraph.ViewModels
{
    internal class DelegateCommand : ICommand
    {
        private Action addEntityExecute;

        public DelegateCommand(Action addEntityExecute)
        {
            this.addEntityExecute = addEntityExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            addEntityExecute();
        }
    }
}