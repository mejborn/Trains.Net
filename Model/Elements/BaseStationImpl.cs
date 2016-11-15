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
        bool IBaseStation.AddConnection(IBaseConnection connection)
        {
            throw new NotImplementedException();
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
