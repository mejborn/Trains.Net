using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class UndoAndRedoImpl
    {
        public static readonly UndoAndRedoImpl UndoAndRedoInstance = new UndoAndRedoImpl();

        private readonly LinkedList<IUndoRedoObject> _undoList = new LinkedList<IUndoRedoObject>();
        private readonly LinkedList<IUndoRedoObject> _redoList = new LinkedList<IUndoRedoObject>();

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
            _redoList.Clear();
            _undoList.AddFirst(new UndoRedoObject<T>(o, a, prop));
        }

        public object Undo()
        {
            var o = _undoList.First.Value;
            _undoList.RemoveFirst();
            _redoList.AddFirst(o);
            return o;
        }

        public object Redo()
        {
            IUndoRedoObject o = _redoList.First?.Value;
            _redoList.RemoveFirst();
            _undoList.AddFirst(o);
            return o;
        }
     
    }

    public interface IUndoRedoObject
    {
        object O { get; }
        UndoAndRedoImpl.Actions A { get; }
    }
    public class UndoRedoObject<T> : IUndoRedoObject
    {
        public object O { get; }
        public UndoAndRedoImpl.Actions A { get; }
        public T Prop { get; }

        public UndoRedoObject(object o, UndoAndRedoImpl.Actions a, T prop)
        {
            O = o;
            A = a;
            Prop = prop;
        }
    }
}



