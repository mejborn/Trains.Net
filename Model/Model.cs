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
            Stations.Add(new BaseStation());
        }
        public List<IBaseStation> GetStations()
        {
            return Stations;
        }
    }

    public interface IBaseStation
    {
        int Left { get; set; }
        int Top { get; set; }
    }

    public class BaseStation : IBaseStation
    {
        public int Left { get; set; }
        public int Top { get; set; } 
        public BaseStation()
        {
            this.Left = 10;
            this.Top = 10;
        }
    }
}
