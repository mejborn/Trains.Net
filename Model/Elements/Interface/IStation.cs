using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Implementation;

namespace Model.Elements.Interface
{

    public interface StationViewModel : IBaseNode
    {
        string Name { get; set; }
        List<ConnectionPointImpl> ConnectionPoints { get; }

        void AddConnectionPoint(string v);
    }
}
