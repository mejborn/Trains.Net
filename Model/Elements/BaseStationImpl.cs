using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseStationImpl : IBaseStation
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public string Color { get; set; }
        public List<IBaseConnection> Connections { get; }

        public BaseStationImpl()
        {
            this.Left = 10;
            this.Top = 10;
            this.Color = "Red";
        }
    }
}
