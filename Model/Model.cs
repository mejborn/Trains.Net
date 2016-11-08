using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrainsModel
{
    public interface IModel
    {
        ObservableCollection<IBaseStation> GetStations();
        void addNote();
    }

    public class Model : IModel
    {
        ObservableCollection<IBaseStation> Stations;
        public Model()
        {
            Stations = new ObservableCollection<IBaseStation>();
            Stations.Add(new BaseStation());
        }

        public void addNote()
        {
            BaseStation station = new BaseStation();
            station.Left = 50;
            station.Top = 50;
            Stations.Add(station);
            Console.WriteLine("Added station");
        }

        public ObservableCollection<IBaseStation> GetStations()
        {
            return Stations;
        }
        
    }

    public interface IBaseStation
    {
        int Left { get; set; }
        int Top { get; set; }
        List<BaseStation> Connections { get; }
    }

    public class BaseStation : IBaseStation
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public List<BaseStation> Connections { get; }

        public BaseStation()
        {
            this.Left = 10;
            this.Top = 10;
        }
    }

}
