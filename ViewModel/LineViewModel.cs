using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Model;
using Model.Elements.Interface;

namespace ViewModel
{
    public class LineViewModel : ViewModelBase
    {
        private string _name;
        public string Name { get { return _name;} set { _name = value; RaisePropertyChanged(); } }
        private List<IStation> Stations = new List<IStation>();

        public LineViewModel(string name,List<IStation> stations)
        {
            Name = name;
            Stations = stations;
            foreach(var station in Stations)
            {
                Console.WriteLine(station.Name);
            }
            
        }
    }
}
