using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Elements
{
    [XmlType("Station")]
    public class StationImpl : BaseElementImpl, IStation
    {

        public string Color { get; set; } = "Red";
        public string Name { get; set; }
        [XmlArray("Connections"), XmlArrayItem("Connection")]
        public List<BaseConnectionImpl> Connections { get; } = new List<BaseConnectionImpl>();
        [XmlArray("ConnectionPoints"), XmlArrayItem("ConnectionPoint")]
        public List<ConnectionPointImpl> ConnectionPoints { get; } = new List<ConnectionPointImpl>();

        public StationImpl() { }

        public StationImpl(string name, double left, double top)
        {
            Width = 200;
            Height = 100;
            Name = name;
            Left = left;
            Top = top;
        }

        public void AddConnection(BaseConnectionImpl connection)
        {
            Connections.Add(connection);
        }

        public void AddConnectionPoint(string v)
        {
            ConnectionPoints.Add(new ConnectionPointImpl() { AssociatedSide = v } );
        }
    }
}
