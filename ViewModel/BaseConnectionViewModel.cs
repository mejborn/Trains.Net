using Model.Elements;
using System.Windows;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        IBaseConnection Connection;
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public BaseConnectionViewModel(IBaseConnection Element) : base(Element)
        {
            Connection = Element;
            Left2 = Element.Left2;
            Top2 = Element.Top2;
        }
        public void UpdatePos()
        {
            Left2 = Connection.Left2;
            Top2 = Connection.Top2;
        }
    }
}
