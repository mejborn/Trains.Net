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
        
        public StationImpl(string name, double left, double top)
        {
            Connections = new List<IBaseConnection>();
            Width = 200;
            Height = 100;
            this.Name = name;
            this.Left = left;
            this.Top = top;
            
            ConnectionPoints = new List<IBaseElement>();
            
        }

        public void AddConnection(IBaseConnection connection)
        {
            Connections.Add(connection);
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
