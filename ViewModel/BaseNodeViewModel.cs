using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class BaseNodeViewModel : BaseElementViewModel
    {
        public string Color { get; set; }
        protected Boolean ElementIsCaught = false;
        protected UIElement CaughtElement;
        private IBaseNode BaseNode;

        public BaseNodeViewModel(IBaseNode Element) : base(Element)
        {
            Color = Element.Color;
            BaseNode = Element;
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
            if (CaughtElement != null)
                CaughtElement.ReleaseMouseCapture();
            CaughtElement = null;
            ElementIsCaught = false;
            
        }
        private void MouseMove(MouseEventArgs e)
        {
            if (CaughtElement != null && ElementIsCaught)
            {
                Top += e.GetPosition(CaughtElement).Y - CaughtElement.RenderSize.Height / 2;
                Left += e.GetPosition(CaughtElement).X - CaughtElement.RenderSize.Width / 2;
                UpdateConnections();
            }
        }

        public void UpdateConnections()
        {
            
            foreach (IBaseConnection connection in BaseNode.Connections)
            {
                System.Console.WriteLine("Found connection");
                connection.Left = Left;
                connection.Top = Top;
            }
        }
    }
}
