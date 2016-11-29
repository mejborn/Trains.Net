using Model.Elements.Implementation;
using Model.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class StationInfoImpl : BaseElementImpl, IStationInfo
    {

        public StationInfoImpl(IStation Station)
        {
            Left = Station.Left;
            Top = Station.Top;
        }
    }
}
