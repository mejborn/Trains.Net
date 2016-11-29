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
        
        protected bool ElementIsCaught;
        protected UIElement CaughtElement;
        private IBaseNode _baseNode;
        

        public NodeViewModel(IBaseNode element) : base(element)
        {
            Color = element.Color;
            Opacity = 1;
            _baseNode = element;
        }
        public ICommand DeltaCommand => new RelayCommand<Vector>(v => { Top += v.Y; Left += v.X;});
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
            
            foreach (IBaseConnection connection in _baseNode.Connections)
            {
                Console.WriteLine("Found connection");
                connection.Left = Left;
                connection.Top = Top;
            }
        }
    }
}
