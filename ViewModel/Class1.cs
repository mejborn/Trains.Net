using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace ViewModel
{
    public class ViewModel
    {
        private IDataModel idm;

        public ViewModel()
        {
            idm.GetStations();
            idm.RemoveStation(new Station());
        }
    }
}
