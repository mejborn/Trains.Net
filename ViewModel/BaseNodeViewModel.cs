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
        public ICommand MouseMoveCommand => new RelayCommand<Vector>(v => { Top += v.Y; Left += v.X; UpdateViewModel(); });

        private void UpdateViewModel()
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
            System.Console.Write("Update connection positions");
            
            foreach (IBaseConnection connection in BaseNode.Connections)
            {
                System.Console.WriteLine("Found connection");
                connection.Left = Left;
                connection.Top = Top;
            }
        }
    }
}
