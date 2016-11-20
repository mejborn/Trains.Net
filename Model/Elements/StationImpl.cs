using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class StationImpl : BaseElementImpl, IStation
    {
        public string Color { get; set; } = "Red";
        public string Name { get; set; }
        public List<IBaseConnection> Connections { get; }
        public List<IConnectionPoint> ConnectionPoints { get; }
        
        public StationImpl(string name, double left, double top)
        {
            Connections = new List<IBaseConnection>();
            Width = 200;
            Height = 100;
            Name = name;
            Left = left;
            Top = top;
            
            ConnectionPoints = new List<IConnectionPoint>();
            
        }

        public void AddConnection(IBaseConnection connection)
        {
            Connections.Add(connection);
        }

        public void AddConnectionPoint(string v)
        {
            ConnectionPoints.Add(new ConnectionPointImpl() { AssociatedSide = v } );
        }
    }
}
