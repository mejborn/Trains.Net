﻿using GalaSoft.MvvmLight.Command;
using Model.Elements;
using System;
using System.Windows;
using System.Windows.Input;
using Model.Elements.Interface;

namespace ViewModel
{
    public class NodeViewModel : BaseElementViewModel
    {
       
        public string Color { get { return BaseNode.Color; } set
        {
            BaseNode.Color = value; RaisePropertyChanged(); } } 

        protected bool ElementIsCaught;
        protected UIElement CaughtElement;
        public IBaseNode BaseNode { get; set; }

        public NodeViewModel(IBaseNode element) : base(element)
        {
            BaseNode = element;
            Color = element.Color;
            Opacity = 1;
            
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
            foreach (IBaseConnection connection in BaseNode.Connections)
            {
                connection.Left = Left;
                connection.Top = Top;
            }
        }
    }
}
