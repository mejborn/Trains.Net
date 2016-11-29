using Model.Elements;
using System.Windows;
using Model.Elements.Interface;

namespace ViewModel
{
    public class BaseConnectionViewModel : BaseElementViewModel
    {
        IBaseConnection Connection;

        protected double _x1;
        protected double _x2;
        protected double _y1;
        protected double _y2;

       
        public double X1 { get { return _x1; } set { _x1 = value; Connection.X1 = value; RaisePropertyChanged(); } }
        public double X2 { get { return _x2; } set { _x2 = value; Connection.X2 = value; RaisePropertyChanged(); } }
        public double Y1 { get { return _y1; } set { _y1 = value; Connection.Y1 = value; RaisePropertyChanged(); } }
        public double Y2 { get { return _y2; } set { _y2 = value; Connection.Y2 = value; RaisePropertyChanged(); } }

        public string Color { get; set; }
        public BaseConnectionViewModel(IBaseConnection element) : base(element)
        {
            Connection = element;
            Top = element.Left;
            Left = element.Top;
            X1 = element.X1;
            X2 = element.X2;
            Y1 = element.Y1;
            Y2 = element.Y2;
            Color = element.Color;
        }
        public void UpdatePos()
        {
            X1 = Connection.X1;
            X2 = Connection.X2;
            Y1 = Connection.Y1;
            Y2 = Connection.Y2;
          
        }
    }
}
