using Model.Elements;
using System.Windows;
using Model.Elements.Interface;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        IBaseConnection Connection;
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public string Color { get; set; }
        public BaseConnectionViewModel(IBaseConnection element) : base(element)
        {
            Connection = element;
            Left2 = element.Left2;
            Top2 = element.Top2;
            Color = element.Color;
        }
        public void UpdatePos()
        {
            Left2 = Connection.Left2;
            Top2 = Connection.Top2;
        }
    }
}
