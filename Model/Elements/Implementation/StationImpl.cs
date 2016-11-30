using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    [XmlType("Station")]
    public class StationImpl : BaseElementImpl, IStation
    {

        public string Color { get; set; } = "Red";
        public double Opacity { get; set; } = 1;
        public string Name { get; set; }
        [XmlArray("Connections"), XmlArrayItem("Connection")]
        public List<BaseConnectionImpl> Connections { get; } = new List<BaseConnectionImpl>();
        [XmlArray("ConnectionPoints"), XmlArrayItem("ConnectionPoint")]
        public List<ConnectionPointImpl> ConnectionPoints { get; } = new List<ConnectionPointImpl>();

        public StationImpl() { }

        public StationImpl(string name, double left, double top)
        {
            Width = 100;
            Height = 50;
            Name = name;
            Left = 100;
            Top = 100;
        }

        public void AddConnection(BaseConnectionImpl connection)
        {
            Connections.Add(connection);
        }

        public ConnectionPointImpl AddConnectionPoint(string v)
        {
            var allowedAmount = (v == "Top" || v == "Bottom") ? 10 : 5;
            if (ConnectionPoints.FindAll(e => e.AssociatedSide == v).Count ==  allowedAmount)
            { throw  new Exception("The number of connection points on the " + v.ToLower() + " must not exceed " + allowedAmount);}
            ConnectionPointImpl cp = new ConnectionPointImpl() {AssociatedSide = v};
            ConnectionPoints.Add(cp );
            return cp;
        }
    }
}
