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
        public string Color { get; set; }
        public string Name { get; set; }
        public List<IBaseConnection> Connections { get; }
        
        public BaseStationImpl(String name, double left, double top)
        {
            Connections = new List<IBaseConnection>();
            this.Name = name;
            this.Left = left;
            this.Top = top;
            this.Color = "Red";
            

        }

        public void AddConnection(IBaseConnection connection)
        {
            Connections.Add(connection);
         }

    }
}
