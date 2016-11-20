using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;

namespace ViewModel
{
    public class ConnectionPointViewModel : BaseElementViewModel
    {
        public string Color { get; } = "White";
        public new double Top { get { return top-5; } set { top = value; Element.Top = value; RaisePropertyChanged(); } }
        public new double Left { get { return left-5; } set { left = value; Element.Left = value; RaisePropertyChanged(); } }
        public ConnectionPointViewModel(IBaseElement Element) : base(Element)
        {
            
        }
    }
}
