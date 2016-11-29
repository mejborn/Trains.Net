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
        public StationInfoViewModel(IStationInfo element) : base(element)
        {
            Console.WriteLine("Constructed StationInfoViewModel");
            Left = element.Left+50;
            Top = element.Top+10;
        }
    }
}
