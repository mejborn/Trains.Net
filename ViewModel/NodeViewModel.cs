using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.Windows;
using System.Windows.Input;
using Model.Elements.Interface;

namespace ViewModel
{
    public class NodeViewModel : BaseElementViewModel
    {
        public string Color { get; set; }
        public double Opacity { get; set; }
        protected Boolean ElementIsCaught = false;
        protected UIElement CaughtElement;
        private IBaseNode BaseNode;

        public NodeViewModel(IBaseNode Element) : base(Element)
        {
            Color = Element.Color;
            Opacity = Element.Opacity;
            BaseNode = Element;
        }
        public ICommand DeltaCommand => new RelayCommand<Vector>(v => { Top += v.Y; Left += v.X; });
        private void UpdateViewModel()
        {
            if (CaughtElement != null)
                CaughtElement.ReleaseMouseCapture();
            CaughtElement = null;
            ElementIsCaught = false;
            
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
