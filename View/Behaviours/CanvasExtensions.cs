using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace View.Behaviours
{
    public static class CanvasExtensions
    {
        public static readonly DependencyProperty EnableDragDropBehaviorProperty =
            DependencyProperty.RegisterAttached(
                   "EnableDragDropBehavior",
                   typeof(ICommand),
                   typeof(CanvasExtensions),
                   new UIPropertyMetadata(null, OnEnableDragDropChanged));

        public static void SetEnableDragDropBehavior(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(EnableDragDropBehaviorProperty, value);
        }

        private static void OnEnableDragDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Console.WriteLine("Set OnEnableDragDropChanged");
        }
    }
}
