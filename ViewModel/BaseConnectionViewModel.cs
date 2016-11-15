using Model.Elements;
using System.Windows;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        public IBaseNode Node1, Node2;
        private BaseNodeViewModel To, From;
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public BaseConnectionViewModel(IBaseConnection Element) : base(Element)
        {
            Left2 = Element.Left2;
            Top2 = Element.Top2;
        }
    }
}
