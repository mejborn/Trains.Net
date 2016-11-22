using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace ViewModel.UndoAndRedo
{
    public class UndoAndRedoController
    {
        private static readonly UndoAndRedoController thisClass = new UndoAndRedoController();
        private LinkedList<IUndoAndRedoCommand> UndoStack = new LinkedList<IUndoAndRedoCommand>();
        private LinkedList<IUndoAndRedoCommand> RedoStack = new LinkedList<IUndoAndRedoCommand>();

        // Ensures only one instance of this can be created
        
        public static UndoAndRedoController instanceOfUndoRedo => thisClass;

        public RelayCommand UndoCommand => new RelayCommand(UndoOperation, CanUndo);
        public RelayCommand RedoCommand => new RelayCommand(RedoOperation, CanRedo);

        private bool CanUndo() => UndoStack.Any();
        private bool CanRedo() => RedoStack.Any();

        public void ResetStacks()
        {
            UndoStack.Clear();
            RedoStack.Clear();
            Refresh();
        }

        public void AddToStackAndExecute(IUndoAndRedoCommand cmd)
        {
            if (UndoStack.Count == 20)
            {
                UndoStack.RemoveLast();
            }
            UndoStack.AddFirst(cmd);

            cmd.ExecuteCommand();
            //Console.WriteLine("HMMMMM: " + UndoStack.Count);
            //Console.WriteLine("HMMMMM: " + UndoStack.Any());
            //Console.WriteLine("HMMMMM: " + RedoStack.Count);
            Refresh();
        }

        public void RedoOperation()
        {
            IUndoAndRedoCommand cmd = RedoStack.First();
            RedoStack.RemoveFirst();
            if (UndoStack.Count == 20)
            {
                UndoStack.RemoveLast();
            }
           UndoStack.AddFirst(cmd);
           cmd.ExecuteCommand();
           Refresh();
        }

        public void UndoOperation()
        {
            IUndoAndRedoCommand cmd = UndoStack.First();
            UndoStack.RemoveFirst();
            if (RedoStack.Count == 20)
            {
                RedoStack.RemoveLast();
            }
            RedoStack.AddFirst(cmd);
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
