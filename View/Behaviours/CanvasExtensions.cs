using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ViewModel;

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
        public static ICommand GetEnableDragDropBehavior(UIElement uiElement)
        {
            return uiElement.GetValue(EnableDragDropBehaviorProperty) as ICommand;
        }

        private static void OnEnableDragDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null)
                return;
            var parentCanvas = FindParent<Window>(element);
            if (parentCanvas == null)
                return;

            var mouseDown =
                Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    action => ((sender, args) => action(args)),
                    ev => element.MouseDown += ev,
                    ev => element.MouseDown -= ev
                ).Select(x => x.GetPosition(parentCanvas));

            var mouseUp =
                Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    action => ((sender, args) => action(args)),
                    ev => parentCanvas.MouseUp += ev,
                    ev => parentCanvas.MouseUp -= ev
                );

            var mouseLeave =
                Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                    action => ((sender, args) => action(args)),
                    ev => parentCanvas.MouseLeave += ev,
                    ev => parentCanvas.MouseLeave -= ev
                );


            var mouseMove =
                Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                    action => ((sender, args) => action(args)),
                    ev => parentCanvas.MouseMove += ev,
                    ev => parentCanvas.MouseMove -= ev
                ).Select(x => x.GetPosition(parentCanvas));


            var mouseMoves = mouseMove.Skip(1).Zip(mouseMove, (left, right) => left - right);
            var endDrag = mouseUp.Merge(mouseLeave);

            // mouseDown.SelectMany(md => mouseMoves.TakeUntil(mouseUp));
            var mouseDrags =
                from md in mouseDown
                from delta in mouseMoves.TakeUntil(endDrag)
                select delta;
            
            var command = GetEnableDragDropBehavior(element);

            mouseDrags.ObserveOn(element.Dispatcher).Subscribe(pos => 
            {
                if(command != null && command.CanExecute(pos))
                    command.Execute(pos);
            }
            );
            command.Execute(null);
            
        }
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
