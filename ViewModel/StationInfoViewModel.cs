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
        private IStation element;

        public List<ConnectionPointImpl> Connections { get; private set; }
        public StationInfoViewModel(IStation Station) : base(Station)
        {
            //this.Left = Station.Left + 50;
            //this.Top = Station.Top - 10;
        }
    }
}
