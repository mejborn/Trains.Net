using Model.Elements.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class StationInfoImpl : BaseElementImpl, IStationInfo
    {
        bool visible;

        public StationInfoImpl(double left, double top)
        {
            Left = left;
            Top = top;
        }

        public bool show()
        {
            Console.WriteLine("StationInfo.show()");
            return visible = true;
        }
        public bool hide()
        {
            Console.WriteLine("StationInfo.hide()");
            return visible = false;
        }
    }
}
