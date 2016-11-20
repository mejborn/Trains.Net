using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{

    public interface IStation : IBaseNode
    {
        string Name { get; set; }
        List<IBaseElement> ConnectionPoints { get; }

        void AddConnectionPoint(string v);
    }
}
