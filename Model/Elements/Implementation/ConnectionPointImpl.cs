using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    public class ConnectionPointImpl : BaseElementImpl,IConnectionPoint
    {
        public BaseConnectionImpl Connection { get; set; }
        public StationImpl Station { get; set; }
        public string AssociatedSide { get; set; }
        public ConnectionPointImpl() {}
    }
}
