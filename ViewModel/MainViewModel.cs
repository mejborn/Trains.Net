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
        BaseElementViewModel selectedElement;
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand addNode => new RelayCommand(AddNode);
        public ICommand addStation => new RelayCommand(AddStation);
        public ICommand AddConnectionPoint => new RelayCommand<string>(v =>
        {
            StationViewModel station = selectedElement as StationViewModel;
            if (station != null)
            {
                station.AddConnectionPoint(v);
                RefreshElements();
            }
            else
                throw new NotImplementedException();
        });


        public MainViewModel()
        {
            iModel = new ModelImpl();
            foreach (var Element in iModel.GetElements())
            {
                BaseElementViewModel viewModel = Util.CreateViewModel(Element);
                viewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(viewModel);
            }
        }

        private void OnHasBeenSelected(object sender, EventArgs e)
        {
            BaseElementViewModel element = sender as BaseElementViewModel;
            if (element != null)
                selectedElement = element;
            else
                throw new NotImplementedException();
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
            foreach (var Element in iModel.GetElements()) { Elements.Add(Util.CreateViewModel(Element)); }
        }

        
        
    }
}