using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.UndoAndRedo
{
    public interface IUndoAndRedoCommand
    {
        void ExecuteCommand();
        void UnExecuteCommand();


    }
}
