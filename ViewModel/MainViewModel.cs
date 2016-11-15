using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TrainsModel;

namespace TrainsViewModel.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        IModel iModel;
        Point startPos;
        Point endPos;

        public ObservableCollection<IBaseStation> Stations { get; private set; }
        public ICommand addNote => new RelayCommand(iModel.addNote);
        public ICommand OnMouseLeftButtonDownCommand => new RelayCommand<DragEventArgs>(OnMouseLeftButtonDown);
        public ICommand OnMouseMoveCommand => new RelayCommand<UIElement>(OnMouseMove);
        public ICommand OnMouseLeftButtonUpCommand => new RelayCommand<DragEventArgs>(OnMouseLeftButtonUp);
        public MainViewModel()
        {
            iModel = new Model();
            Stations = iModel.GetStations();
        }

        private void OnMouseLeftButtonDown(DragEventArgs e)
        {
            System.Console.WriteLine("Got a mouse click!");
            startPos = new Point(X, Y);

        }
        private void OnMouseMove(UIElement e)
        {
            System.Console.WriteLine("Got move command!");
        }
        private void OnMouseLeftButtonUp(DragEventArgs obj)
        {
            System.Console.WriteLine("Released mouse!");
        }


    }
}