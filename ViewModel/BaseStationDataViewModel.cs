using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;

namespace ViewModel
{
    class BaseStationDataViewModel : BaseElementViewModel
    {
        private string Color { get; set; }
        IBaseStationData BaseStationData;

        public BaseStationDataViewModel(IBaseStationData Element) : base(Element)
        {
            BaseStationData = Element;
            Console.WriteLine("Show station data");
        }
    }
}
