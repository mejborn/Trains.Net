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
    public static class MoveElementBehavior
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MoveElementBehavior), new PropertyMetadata(OnCommandChanged));



        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MoveElementBehavior), new PropertyMetadata(null));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("Entered OnCommandChanged");
            Selector s = d as Selector;
            if (s == null) return;
            s.SelectionChanged += OnSelection;
        }

        static void OnSelection(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Entered OnSelection");
            Selector s = sender as Selector;
            ICommand cmd = s.GetValue(MoveElementBehavior.CommandProperty) as ICommand;
            object parm = s.GetValue(MoveElementBehavior.CommandParameterProperty);
            if (cmd != null && cmd.CanExecute(parm))
                cmd.Execute(parm);
        }
    }
}
