using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Model.Elements.Interface;

namespace ViewModel
{
    public abstract class BaseElementViewModel : ViewModelBase
    {
        
        public IBaseElement Element { get; }

        protected double top;
        protected double left;
        private int _widht;
        private int _height;
        private double _opacity;
        public Point PrevPos;

        public ICommand DownCommand => new RelayCommand(() => { PrevPos = new Point(top,left); OnHasBeenSelected(null); });
        public ICommand SelectStationCommand => new RelayCommand(() => { OnHasBeenSelected(null); });
        public ICommand UpCommand => new RelayCommand(() => { OnHasBeenReleased(null); });
        public double Opacity { get { return _opacity; } set { _opacity = value; RaisePropertyChanged(); } }
        public double Top { get { return top; } set { top = value; Element.Top = value; RaisePropertyChanged(); } }
        public double Left { get { return left; } set { left = value; Element.Left = value; RaisePropertyChanged(); } }
        public int Width { get { return _widht; } set { _widht = value; Element.Width = value; RaisePropertyChanged(); } }
        public int Height { get { return _height; } set { _height = value; Element.Height = value; RaisePropertyChanged(); } }
        protected BaseElementViewModel(IBaseElement element)
        {
            Element = element;
            Top = element.Top;
            Left = element.Left;
            Width = element.Width;
            Height = element.Height;
        }
        public event EventHandler HasBeenSelected;
        public event EventHandler HasBeenReleased;

        protected virtual void OnHasBeenReleased(EventArgs e)
        {
            HasBeenReleased?.Invoke(this,e);
        }

        protected virtual void OnHasBeenSelected(EventArgs e)
        {
            HasBeenSelected?.Invoke(this, e);
        }

        public BaseElementViewModel ShallowCopy()
        {
            return (BaseElementViewModel)MemberwiseClone();
        }
    }
}
