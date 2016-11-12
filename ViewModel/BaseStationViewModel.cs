using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using System.Windows.Input;

namespace ViewModel
{
    public class BaseStationViewModel : BaseNodeViewModel
    {
        public string Name { get; set; }
        public BaseStationViewModel(IBaseStation Element) : base(Element)
        {
            Name = Element.Name;
        }
    }
}
