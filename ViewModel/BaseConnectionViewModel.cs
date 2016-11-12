using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model.Elements;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public BaseConnectionViewModel(IBaseConnection Element) : base(Element)
        {
            Left2 = Element.Left2;
            Top2 = Element.Top2;
        }
    }
}
