using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public abstract class BaseElementViewModel : ViewModelBase
    {
        
        protected IBaseElement Element;

        private double top;
        private double left;
        public double Top { get { return top; } set { top = value; Element.Top = Top; RaisePropertyChanged(); } }
        public double Left { get { return left; } set { left = value; Element.Left = Left; RaisePropertyChanged(); } }
        protected BaseElementViewModel(IBaseElement Element)
        {
            this.Element = Element;
            Top = Element.Top;
            Left = Element.Left;
        }
        

    }
}
