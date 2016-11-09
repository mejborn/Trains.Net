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
        ObservableCollection<IBaseElement> Stations;
        public ModelImpl()
        {
            Stations = new ObservableCollection<IBaseElement>();
            Stations.Add(new BaseStationImpl());
        }

        public void addNote()
        {
            IBaseConnection station = new BaseConnectionImpl();
            Stations.Add(station);
            Console.WriteLine("Added BaseConnection");
        }
        public void addStation()
        {
            IBaseStation station = new BaseStationImpl();
            station.Left = 50;
            station.Top = 50;
            station.Color = "Green";
            Stations.Add(station);
            Console.WriteLine("Added station");
        }

        public ObservableCollection<IBaseElement> GetStations()
        {
            return Stations;
        }
        
    }
}
