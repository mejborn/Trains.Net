using Model;
using Model.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrainsModel
{
    public class ModelImpl : IModel
    {
        ObservableCollection<IBaseElement> Elements;
        public ModelImpl()
        {
            Elements = new ObservableCollection<IBaseElement>();
            Elements.Add(new BaseStationImpl("First"));
        }

        public void addNode()
        {
            IBaseNode Node = new BaseNodeImpl()
            {
                Left = 100,
                Top = 100
            };
            Elements.Add(Node);
        }
        public void addStation()
        {
            IBaseStation station = new BaseStationImpl("Second");
            station.Left = 50;
            station.Top = 50;
            station.Color = "Green";
            Elements.Add(station);

            IBaseConnection connection = new BaseConnectionImpl()
            {
                Left = Elements[0].Left,
                Top = Elements[0].Top,
                Left2 = station.Left,
                Top2 = station.Top
            };
            Elements.Add(connection);


        }

        public ObservableCollection<IBaseElement> GetStations()
        {
            return Elements;
        }
        
    }
}
