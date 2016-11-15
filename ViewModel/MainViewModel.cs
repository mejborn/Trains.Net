using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using Model.Elements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IModel iModel;
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand addNode => new RelayCommand(AddNode);
        public ICommand addStation => new RelayCommand(AddStation);
        
        public MainViewModel()
        {
            iModel = new ModelImpl();
            foreach (var Element in iModel.GetElements()) { Elements.Add(CreateViewModel(Element)); }
        }

        private void AddNode()
        {
            iModel.AddNode(10,10);
            RefreshElements();
        }

        private void AddStation()
        {
            iModel.AddStation("Fredensborg", 20, 20);

            RefreshElements();
        }
        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var Element in iModel.GetElements()) { Elements.Add(CreateViewModel(Element)); }
            System.Console.WriteLine(Elements.Count);
        }

        static Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>
        {
            {typeof(BaseStationImpl), typeof(BaseStationViewModel)},
            {typeof(BaseNodeImpl), typeof(BaseNodeViewModel)},
            {typeof(BaseConnectionImpl), typeof(BaseConnectionViewModel) }, 
        };

        static BaseElementViewModel CreateViewModel(IBaseElement Element)
        {
            return Activator.CreateInstance(TypeMap[Element.GetType()],Element) as BaseElementViewModel;
        }
        
    }
}