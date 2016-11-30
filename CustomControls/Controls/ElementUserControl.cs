using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ViewModel;

namespace CustomControls
{
    public class ElementUserControl : UserControl
    {
        private Point lastPos;
        private Window parentView;
        private ElementUserControl _selectedControl;
        public ElementUserControl()
        {
            if (parentView == null)
                parentView = Utility.ViewUtilities.FindParent<Window>(this);
            MouseUp += ElementUserControl_MouseUp;
            MouseDown += ElementUserControl_MouseDown;
        }

        private void ElementUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _selectedControl = sender as ElementUserControl;
            if(parentView == null)
                parentView = Utility.ViewUtilities.FindParent<Window>(this);
            parentView.MouseMove += ElementUserControl_MouseMove;
            lastPos = e.GetPosition(parentView);
            if (DownCommand != null && DownCommand.CanExecute(null))
                DownCommand?.Execute(null);
        }

        private void ElementUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPos = e.GetPosition(parentView);
            if (currentPos.X <= 10 || 
                currentPos.X >= parentView.Width - 20 ||
                currentPos.Y <= 10 ||
                currentPos.Y >= parentView.Height - 50)
            {
                parentView.MouseMove -= ElementUserControl_MouseMove;
            }
            else if(DeltaCommand != null && DeltaCommand.CanExecute(currentPos - lastPos))
            {
                DeltaCommand.Execute(currentPos - lastPos);
            }
            lastPos = currentPos;
        }

        private void ElementUserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (parentView != null)   
                parentView.MouseMove -= ElementUserControl_MouseMove;
            if (UpCommand != null && UpCommand.CanExecute(null))
                UpCommand.Execute(null);
        }


        public ICommand UpCommand
        {
            get { return (ICommand)GetValue(UpCommandProperty); }
            set { SetValue(UpCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpCommandProperty =
            DependencyProperty.Register("UpCommand", typeof(ICommand), typeof(ElementUserControl), new PropertyMetadata(null));


        public ICommand DeltaCommand
        {
            get { return (ICommand)GetValue(DeltaCommandProperty); }
            set { SetValue(DeltaCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeltaCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeltaCommandProperty =
            DependencyProperty.Register("DeltaCommand", typeof(ICommand), typeof(ElementUserControl), new PropertyMetadata(null));


        public ICommand DownCommand
        {
            get { return (ICommand)GetValue(DownCommandProperty); }
            set { SetValue(DownCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownCommandProperty =
            DependencyProperty.Register("DownCommand", typeof(ICommand), typeof(ElementUserControl), new PropertyMetadata(null));

        
    }
}
