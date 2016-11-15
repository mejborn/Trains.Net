using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{

    public interface IBaseStation : IBaseNode
    {
        string Name { get; set; }

        new bool AddConnection(IBaseConnection connection);
    }
}
