using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace Model.UndoAndRedo
{
    public class UndoAndRedoController
    {
        private static readonly UndoAndRedoController thisClass = new UndoAndRedoController();
        private Stack<IUndoAndRedoCommand> UndoStack = new Stack<IUndoAndRedoCommand>();
        private Stack<IUndoAndRedoCommand> RedoStack = new Stack<IUndoAndRedoCommand>();

        // Ensures only one instance of this can be created
       
        public static UndoAndRedoController instanceOfUndoRedo => thisClass;

        public RelayCommand UndoCommand => new RelayCommand(UndoOperation, CanUndo);
        public RelayCommand RedoCommand => new RelayCommand(RedoOperation, CanRedo);

        private bool CanUndo() => UndoStack.Any();
        private bool CanRedo() => RedoStack.Any();

        public void ResetStackAsNewPerspective(IUndoAndRedoCommand cmd)
        {
            UndoStack.Push(cmd);
            RedoStack.Clear();
            cmd.ExecuteCommand();
            Refresh();
        }

        public void RedoOperation()
        {
            IUndoAndRedoCommand cmd = UndoStack.Pop();
            RedoStack.Push(cmd);
            cmd.ExecuteCommand();
            Refresh();
        }

        public void UndoOperation()
        {
            IUndoAndRedoCommand cmd = RedoStack.Pop();
            UndoStack.Push(cmd);
            cmd.ExecuteCommand();
            Refresh();
        }

        public void Refresh()
        {
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

    }
}
