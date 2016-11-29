using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using Model.Elements.Implementation;
using Model.Elements.Interface;

namespace ViewModel
{
    public class StationInfoViewModel : BaseElementViewModel
    {
        public List<String> Connections { get; private set; }

        public StationInfoViewModel(IStationInfo element) : base(element)
        {
            Left = element.Left+50;
            Top = element.Top+10;
            Connections = new List<String>();
        }
        public string size{ get { return Connections.Capacity.ToString(); } }
    }
}
