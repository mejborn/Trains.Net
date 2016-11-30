using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Implementation;

namespace Model.Elements.Interface
{

    public interface IStation : IBaseNode
    {
        string Name { get; set; }
        List<ConnectionPointImpl> ConnectionPoints { get; }

        ConnectionPointImpl AddConnectionPoint(string v);
    }
}
