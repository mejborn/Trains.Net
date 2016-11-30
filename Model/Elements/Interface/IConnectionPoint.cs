using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Implementation;

namespace Model.Elements.Interface
{
    public interface IConnectionPoint : IBaseElement
    {
        StationImpl Station { get; set; }
        BaseConnectionImpl Connection { get; set; }
        string AssociatedSide { get; set; }
        bool IsOccupied { get; set; }
    }
}
