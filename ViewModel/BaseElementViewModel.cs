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
        protected IBaseElement Element;
        protected Boolean ElementIsCaught = false;
        protected UIElement CaughtElement;
        public double Top { get; set; }
        public double Left { get; set; }
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
            if (CaughtElement != null) { ElementIsCaught = true; }
        }

        private void MouseUp(MouseButtonEventArgs e)
        {
            CaughtElement = null; ElementIsCaught = false;
        }

        protected abstract void MouseMove(MouseEventArgs e);

    }
}
