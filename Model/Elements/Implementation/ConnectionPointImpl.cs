using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Interface;
using System.Xml.Serialization;

namespace Model.Elements.Implementation
{
    [XmlType("ConnectionPointImpl")]
    public class ConnectionPointImpl : BaseElementImpl,IConnectionPoint
    {
        public BaseConnectionImpl Connection { get; set; }
        public StationImpl Station { get; set; }
        public string AssociatedSide { get; set; }
        public bool IsOccupied { get; set; }
        public ConnectionPointImpl() {}
    }
}
