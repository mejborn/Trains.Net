using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Utility
{
    public class UndoAndRedoImpl
    {
        public static readonly UndoAndRedoImpl UndoAndRedoInstance = new UndoAndRedoImpl();

        private  readonly LinkedList<IUndoRedoObject> _undoList = new LinkedList<IUndoRedoObject>();
        private readonly LinkedList<IUndoRedoObject> _redoList = new LinkedList<IUndoRedoObject>();

        private UndoAndRedoImpl() { }

        public bool CanUndo => _undoList.Any();
        public bool CanRedo => _redoList.Any();

        public enum Actions
        {
            Insert,
            Remove,
            Move,
            Update,
            Connect

        }

        public void AddUndoItem<T>(object o, Actions a, T prop)
        {
            _redoList.Clear();
            _undoList.AddFirst(new UndoRedoObject<T>(o, a, prop));
        }

        public object UndoPop()
        {
            var o = _undoList.First.Value;
            _undoList.RemoveFirst();
           return o;
        }

        public void UndoPush(IUndoRedoObject o)
        {
            _undoList.AddFirst(o);
        }
       
        public object RedoPop()
        {
            IUndoRedoObject o = _redoList.First?.Value;
            _redoList.RemoveFirst();
           return o;
        }

        public void RedoPush(IUndoRedoObject o)
        {
            _redoList.AddFirst(o);
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



