using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class ConnectionPointImpl : BaseElementImpl,IConnectionPoint
    {
        public BaseConnectionImpl Connection { get; set; }
        public StationImpl Station { get; set; }
        public string AssociatedSide { get; set; }
        public ConnectionPointImpl() {}
    }
}
