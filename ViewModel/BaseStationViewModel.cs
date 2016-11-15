using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class BaseStationViewModel : BaseNodeViewModel
    {
        public string Name { get; set; }
        public ObservableCollection<BaseElementViewModel> ConnectionPoints { get; } = new ObservableCollection<BaseElementViewModel>();

        public BaseStationViewModel(IBaseStation Element) : base(Element)
        {
            Name = Element.Name;
            foreach (IConnectionPoint ConnectionPoint in Element.ConnectionPoints) { ConnectionPoints.Add(Util.CreateViewModel(ConnectionPoint)); }
        }

    }
}
