using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using Model.Elements.Implementation;
using Model.Elements.Interface;
using Model;

namespace ViewModel
{
    public class StationInfoViewModel
    {
        public IModel Model;
        public List<String> Connections { get; private set; }
        public IStation Station { get; private set; }
        public double Left { get; private set; }
        public double Top { get; private set; }

        public StationInfoViewModel(IStation station, IModel model)
        {
            Model = model;
            Station = station;
            //Station = element;
            Left = 680;
            Top = 15;
            //Initialize list
            Connections = new List<String>();
            updateConnections();
        }
        public string size{ get { return Connections.Count.ToString(); } }

        public void updateConnections()
        {
            foreach (var s in Model.GetStationsConnectedToNode(Station))
            {
                Connections.Add(s.Name);
            }
        }
    }
}
