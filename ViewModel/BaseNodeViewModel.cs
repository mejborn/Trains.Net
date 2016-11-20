using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class BaseNodeViewModel : BaseElementViewModel
    {
        public string Color { get; set; } = "Blue";

        public BaseNodeViewModel(IBaseNode Element) : base(Element)
        {
            Color = Element.Color;
        }
        public ICommand MouseMoveCommand => new RelayCommand<Vector>(v => { Top += v.Y; Left += v.X; UpdateViewModel(); });

        private void UpdateViewModel()
        {
            UpdateConnections();
        }
        private void UpdateConnections()
        {
            //System.Console.Write("Update connection positions");
            
        }
    }
}
