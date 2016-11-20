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
        
        public BaseStationImpl(String name, double left, double top)
        {
            Connections = new List<IBaseConnection>();
            Width = 200;
            Height = 100;
            this.Name = name;
            this.Left = left;
            this.Top = top;
            
            ConnectionPoints = new List<IBaseElement>();
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = 0, Top = Height / 2 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width, Top = Height / 2 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width/2, Top = 0 });
            ConnectionPoints.Add(new ConnectionPointImpl() { Left = Width/2, Top = Height});
        }

        public void AddConnection(IBaseConnection connection)
        {
            Connections.Add(connection);
         }

    }
}
