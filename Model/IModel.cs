using Model.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IModel
    {
        ObservableCollection<IBaseStation> GetStations();
        void addNote();
        void addStation();
    }
}
