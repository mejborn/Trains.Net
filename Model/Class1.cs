using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsModel
{
    public interface IModel
    {
        List<IBaseStation> GetStations();
    }

    public class Model : IModel
    {
        List<IBaseStation> Stations;
        public Model()
        {
            Stations = new List<IBaseStation>();
            Stations.Add(new Station("Test"));
        }
        public List<IBaseStation> GetStations()
        {
            return Stations;
        }
    }

    public interface IBaseStation
    {
        string GetName();
    }

    public class Station : IBaseStation
    {
        string Name;
        public Station(string Name)
        {
            this.Name = Name;
        }
        public string GetName()
        {
            return Name;
        }
    }


}
