using Model.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UndoAndRedo.Implementation
{
    class AddStationCommand : IUndoAndRedoCommand
    {
        private List<IBaseElement> elements;
        private IStation station;

        public AddStationCommand(List<IBaseElement> elements, IStation station )
        {
            this.elements = elements;
            this.station = station;

        }
        
        public void ExecuteCommand()
        {
            elements.Add(station);
        }

        public void UnExecuteCommand()
        {
            elements.Remove(station);
        }
    }
}
