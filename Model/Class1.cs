using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public interface IDataModel
    {
        List<IStation> GetStations();
        void AddStation(IStation station);
        void RemoveStation(Station station);
    }


    public interface IStation { }
    public class Station { }
}
