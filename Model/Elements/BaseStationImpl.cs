using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseStationImpl : BaseElementImpl, IBaseStation
    {
        public string Color { get; set; } = "Red";
        public string Name { get; set; }
        public List<IBaseConnection> Connections { get; }
        public List<IBaseElement> ConnectionPoints { get; }
        
        public BaseStationImpl(String name)
        {
            Left = 10;
            Top = 10;
            Width = 200;
            Height = 100;
            Name = name;

            Connections = new List<IBaseConnection>();
            ConnectionPoints = new List<IBaseElement>();
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = 0, Top = Height / 2 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width, Top = Height / 2 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width/2, Top = 0 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width/2, Top = Height});
        }

        public bool AddConnection(IBaseConnection connection)
        {
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
                return true;
            }
            else return false;

        }
    }
}
