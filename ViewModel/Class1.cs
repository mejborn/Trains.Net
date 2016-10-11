using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class ViewModel
    {
        IDataModel iDataModel;
        public ViewModel()
        {
            List<IBaseStation> Stations = iDataModel.GetStations();
        }
    }
}
