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
        public List<IBaseElement> Connections { get; }
        public bool AddConnection(IBaseConnection connection)
        {
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
                return true;
            }
            else return false;
            
        }


        public BaseStationImpl(String name)
        {
            this.Left = 10;
            this.Top = 10;
            this.Color = "Red";
            this.Name = name;

        }
    }
}
