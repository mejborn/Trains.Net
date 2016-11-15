using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public abstract class BaseElementViewModel : ViewModelBase
    {
        private Point clickPosition;
        protected IBaseElement Element;
        protected Boolean ElementIsCaught = false;
        protected UIElement CaughtElement;
        private double top;
        private double left;
        public double Top { get { return top; } set { top = value; Element.Top = Top; RaisePropertyChanged(); } }
        public double Left { get { return left; } set { left = value; Element.Left = Left; RaisePropertyChanged(); } }
        public BaseElementViewModel(IBaseElement Element)
        {
            this.Element = Element;
            Top = Element.Top;
            Left = Element.Left;
        }

        public ICommand OnMouseDownCommand => new RelayCommand<MouseButtonEventArgs>(MouseDown);
        public ICommand OnMouseUpCommand => new RelayCommand<MouseButtonEventArgs>(MouseUp);
        public ICommand OnMouseMoveCommand => new RelayCommand<MouseEventArgs>(MouseMove);

        private void MouseDown(MouseButtonEventArgs e)
        {
            CaughtElement = e.Source as UIElement;
            CaughtElement.CaptureMouse();
            if (CaughtElement != null) { ElementIsCaught = true; }
        }

        private void MouseUp(MouseButtonEventArgs e)
        {
            if(CaughtElement != null)
                CaughtElement.ReleaseMouseCapture();
            CaughtElement = null;
            ElementIsCaught = false;
        }
        private void MouseMove(MouseEventArgs e)
        {
            if (CaughtElement != null && ElementIsCaught)
            {
                Top += e.GetPosition(CaughtElement).Y - CaughtElement.RenderSize.Height/2;
                Left += e.GetPosition(CaughtElement).X - CaughtElement.RenderSize.Width/2;
            }
        }
    }
}
