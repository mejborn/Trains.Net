using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;

namespace ViewModel.UndoAndRedo
{
    public class UndoAndRedoImpl
    {
        private static readonly UndoAndRedoImpl UndoAndRedoInstance = new UndoAndRedoImpl();

        private LinkedList<IUndoAndRedoCommand> UndoList = new LinkedList<IUndoAndRedoCommand>();
        private LinkedList<IUndoAndRedoCommand> RedoList = new LinkedList<IUndoAndRedoCommand>();

        public UndoAndRedoImpl() :base() { }

        public static UndoAndRedoImpl GetUndoAndredoInstance => UndoAndRedoInstance;

        public bool IsUndoListPopulated => UndoList.Any();
        public bool IsRedoListPopulated => RedoList.Any();


        public
        void AddToListAndExecute(IUndoAndRedoCommand cmd)
        {
            if (UndoList.Count == 20)
            {
                UndoList.RemoveLast();
            }
            UndoList.AddFirst(cmd);
            
            cmd.ExecuteCommand();
        }

        public void RedoOperation()
        {
            IUndoAndRedoCommand cmd = RedoList.First();
            RedoList.RemoveFirst();
            if (UndoList.Count == 20)
            {
                UndoList.RemoveLast();
            }
            UndoList.AddFirst(cmd);
            cmd.ExecuteCommand();
        }

        public void UndoOperation()
        {
            IUndoAndRedoCommand cmd = UndoList.First();
            UndoList.RemoveFirst();
            if (RedoList.Count == 20)
            {
                RedoList.RemoveLast();
            }
            RedoList.AddFirst(cmd);
            cmd.ExecuteCommand();
        }


    }

}



