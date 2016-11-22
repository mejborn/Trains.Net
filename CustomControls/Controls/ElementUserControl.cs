using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls
{
    public class ElementUserControl : UserControl
    {
        private Point lastPos;
        private Window parentView;
        public ElementUserControl()
        {
            MouseUp += ElementUserControl_MouseUp;
            MouseDown += ElementUserControl_MouseDown;
            
        }

        private void ElementUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(parentView == null)
                parentView = FindParent<Window>(this);
            parentView.MouseMove += ElementUserControl_MouseMove;
            lastPos = e.GetPosition(parentView);
        }

        private void ElementUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPos = e.GetPosition(parentView);
            if (DeltaCommand != null && DeltaCommand.CanExecute(currentPos - lastPos))
            {
                DeltaCommand.Execute(currentPos - lastPos);
            }
            lastPos = currentPos;
        }

        private void ElementUserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (parentView != null)   
                parentView.MouseMove -= ElementUserControl_MouseMove;
            if (DownCommand != null && DownCommand.CanExecute(null))
                DownCommand.Execute(null);
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

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }
    }
}
