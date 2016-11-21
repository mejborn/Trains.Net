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

        protected double top;
        protected double left;
        private int widht;
        private int height;

        public ICommand DownCommand => new RelayCommand(() => { OnHasBeenSelected(null); });
        public ICommand UpCommand => new RelayCommand(() => { Console.WriteLine("Upcommand"); });
        

        public double Top { get { return top; } set { top = value; Element.Top = value; RaisePropertyChanged(); } }

        public double Left { get { return left; } set { left = value; Element.Left = value; RaisePropertyChanged(); } }

        public int Width { get { return widht; } set { widht = value; Element.Width = value; RaisePropertyChanged(); } }
        public int Height { get { return height; } set { height = value; Element.Height = value; RaisePropertyChanged(); } }
        protected BaseElementViewModel(IBaseElement Element)
        {
            this.Element = Element;
            Top = Element.Top;
            Left = Element.Left;
            Width = Element.Width;
            Height = Element.Height;
        }
        public event EventHandler HasBeenSelected;

        protected virtual void OnHasBeenSelected(EventArgs e)
        {
            HasBeenSelected?.Invoke(this, e);
        }
    }
}
