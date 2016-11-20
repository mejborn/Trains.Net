using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public interface IConnectionPoint : IBaseElement
    {
        IBaseStation Station { get; set; }
        IBaseConnection Connection { get; set; }
    }
}
