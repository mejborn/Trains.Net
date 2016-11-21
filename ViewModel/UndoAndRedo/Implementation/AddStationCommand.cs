using Model.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel.UndoAndRedo.Implementation
{
    class AddStationCommand : IUndoAndRedoCommand
    {
        private IModel iModel;
        private List<BaseElementImpl> elements;
        private string name;
        private double left;
        private double top;
        private StationImpl station;

        public AddStationCommand(IModel iModel, string name, double left, double top)
        {
            //List<IBaseElement> elements, IStation station 
            this.iModel = iModel;
            this.name = name;
            this.left = left;
            this.top = top;
            //this.station = station;

        }
        
        public void ExecuteCommand()
        {
            station = iModel.AddStation(name, left, top);
            elements = iModel.GetElements();
            //elements.Add(station);
            Console.WriteLine("DAMN!" + elements.Count);
        }

        public void UnExecuteCommand()
        {
            elements.Remove(station);
        }
    }
}
