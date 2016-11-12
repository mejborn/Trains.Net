using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model.Elements;

namespace ViewModel
{
    public class BaseNodeViewModel : BaseElementViewModel
    {
        public string Color { get; set; }
        public BaseNodeViewModel(IBaseNode Element) : base(Element)
        {
            Color = Element.Color;
        }

        protected override void MouseMove(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
