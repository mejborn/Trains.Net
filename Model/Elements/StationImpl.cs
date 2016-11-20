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
        public List<IBaseElement> ConnectionPoints { get; }
        
        public StationImpl(string name)
        {
            Left = 10;
            Top = 10;
            Width = 200;
            Height = 100;
            Name = name;

            Connections = new List<IBaseConnection>();
            ConnectionPoints = new List<IBaseElement>();
            
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

        public void AddConnectionPoint(string v)
        {
            switch (v)
            {
                case "Left":
                    ConnectionPoints.Add(new ConnectionPointImpl() { Left = 0, Top = Height / 2 });
                    break;
                case "Right":
                    ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width, Top = Height / 2 });
                    break;
                case "Top":
                    ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width / 2, Top = 0 });
                    break;
                case "Bottom":
                    ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width / 2, Top = Height });
                    break;
            }
        }
    }
}
