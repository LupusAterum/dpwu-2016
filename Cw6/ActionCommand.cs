using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDoTaskList {
    public class ActionCommand : ICommand {
        private Action<ToDoTask> action;
        public ActionCommand(Action<ToDoTask> action) {
            this.action = action;
        }
        #region ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return true;
        }
        public void Execute(object parameter) {
            action(parameter as ToDoTask);
        }
        #endregion
    }
}
