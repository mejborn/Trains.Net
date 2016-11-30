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
        private ObservableCollection<BaseElementViewModel> Connections = new ObservableCollection<BaseElementViewModel>();

        public LineViewModel(string name,BaseElementViewModel vm, IModel m)
        {
            Name = name;
            foreach (var element in m.GetNodesConnectedToNode(vm.Element as IBaseNode))
            {
                
            }
        }
    }
}
