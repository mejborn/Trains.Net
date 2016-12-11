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
        [XmlIgnore]
        public BaseConnectionImpl Connection { get; set; }
        [XmlIgnore]
        public StationImpl Station { get; set; }
        [XmlIgnore]
        public string AssociatedSide { get; set; }
        public ConnectionPointImpl() {}
    }
}
