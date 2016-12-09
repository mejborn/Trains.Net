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
    public class LineViewModel
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }
        public List<string> StationsInLine = new List<string>();
        private List<IStation> Stations = new List<IStation>();
        public double Left { get; private set; }
        public double Top { get; private set; }

        public LineViewModel(string name, List<IStation> stations)
        {
            _name = name;
            Left = 680;
            Top = 200;
            Stations = stations;
            foreach (var station in Stations)
            {
                Console.WriteLine(station.Name);
            }

        }
        public List<String> GetStations {
            get
            {
                foreach (var station in Stations)
                {
                    StationsInLine.Add(station.Name);
                }

                return StationsInLine;
            }
        }
    }
}
