using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class UndoAndRedoImpl
    {
        public static readonly UndoAndRedoImpl UndoAndRedoInstance = new UndoAndRedoImpl();

        private readonly LinkedList<object> _undoList = new LinkedList<object>();
        private readonly LinkedList<object> _redoList = new LinkedList<object>();

        private UndoAndRedoImpl() { }

        public bool CanUndo => _undoList.Any();
        public bool CanRedo => _redoList.Any();

        public enum Actions
        {
            Insert,
            Remove,
            Move,
            Update
        }

        public void AddUndoItem<T>(object o, Actions a, T prop)
        {
            _undoList.AddFirst(new UndoRedoObject<T>(o, a, prop));
        }

        public object Undo()
        {
            object o = _undoList.First;
            _undoList.RemoveFirst();
            _redoList.AddFirst(o);
            return o;
        }
     
    }

    internal class UndoRedoObject<T>
    {
        protected object O { get; }
        protected UndoAndRedoImpl.Actions A { get; }
        protected T Prop { get; }

        public UndoRedoObject(object o, UndoAndRedoImpl.Actions a, T prop)
        {
            O = o;
            A = a;
            Prop = prop;
        }
    }
}



