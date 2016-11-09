using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Elements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<IBaseElement> Stations { get; private set; }
        public ICommand addNote => new RelayCommand(iModel.addNode);
        public ICommand addStation => new RelayCommand(iModel.addStation);
        public ICommand OnMouseLeftButtonDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
        public MainViewModel()
        {
            iModel = new ModelImpl();
            Stations = iModel.GetStations();
        }

        private void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            System.Console.WriteLine("Got a mouse click!");
        }
        
    }
}