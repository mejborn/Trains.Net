using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
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

        public List<IBaseStation> Stations { get; private set; }
        
        public MainViewModel()
        {
            iModel = new Model();
            Stations = iModel.GetStations();

            this.addNote = new RelayCommand(iModel.addNote);

        }

        public ICommand addNote { get; set; }
    }
}