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
        public int Left2 { get; set; }
        public int Top2 { get; set; }
        public BaseConnectionViewModel(IBaseConnection Element) : base(Element)
        {
            Left2 = Element.Left2;
            Top2 = Element.Top2;
        }

        protected override void MouseMove(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
