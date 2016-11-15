using Model.Elements;
using System.Windows;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        public Point elem1, elem2;
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public BaseConnectionViewModel(IBaseConnection Element) : base(Element)
        {
            Left2 = Element.Left2;
            Top2 = Element.Top2;
            

            System.Console.Write(elem1);
            System.Console.Write(elem2);
        }
    }
}
