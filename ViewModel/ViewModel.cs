using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainsModel;
using System.ComponentModel;

namespace TrainsViewModel
{
    public class ViewModel : INotifyPropertyChanged 
    {
        IModel iModel;
        public List<IBaseStation> Stations { get; private set; }

        public ViewModel()
        {
            iModel = new Model();
            Stations = iModel.GetStations();
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
