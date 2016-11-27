using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Implementation;

namespace Model.Elements.Interface
{
    public interface IBaseNode : IBaseElement
    {
        string Color { get; set; }

        double Opacity { get; set; }
        List<BaseConnectionImpl> Connections { get; }

        void AddConnection(BaseConnectionImpl connection);
    }
}
