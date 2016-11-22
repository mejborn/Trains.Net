using GalaSoft.MvvmLight;
using Model;
using Model.Elements;
using ViewModel.UndoAndRedo;
using ViewModel.UndoAndRedo.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using Utility;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IModel iModel;
        String fileName;
        BaseElementViewModel selectedElement;
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand addNode => new RelayCommand(AddNode);
        public ICommand addStation => new RelayCommand(AddStation);
        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);
        public ICommand AddConnectionPoint => new RelayCommand<string>(v =>
        {
            StationViewModel station = selectedElement as StationViewModel;
            if (station != null)
                station.AddConnectionPoint(v);
            else
                throw new NotImplementedException();
        });
        private UndoAndRedoController undoAndRedoController => UndoAndRedoController.instanceOfUndoRedo;

        public ICommand UndoOperation => undoAndRedoController.UndoCommand;
        public ICommand RedoOperation => undoAndRedoController.RedoCommand;


        public void ShowSaveDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            fileName = sfd.FileName;
        }
        public void ShowLoadDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            fileName = ofd.FileName;
        }

        public void SaveModel()
        {
            if (string.IsNullOrEmpty(fileName))
                ShowSaveDialog();
            if (!string.IsNullOrEmpty(fileName))
                FileIOUtils.SaveObject(iModel,fileName);
        }
        public void SaveModelAs()
        {
            ShowSaveDialog();
            if (!string.IsNullOrEmpty(fileName))
                FileIOUtils.SaveObject(iModel, fileName);
        }
        public void LoadModel()
        {
            ShowLoadDialog();
            if (!string.IsNullOrEmpty(fileName))
            iModel = FileIOUtils.LoadObject<ModelImpl>(fileName);
            RefreshElements();
        }
        public string inputText { get; private set; }

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
            {
                selectedElement = element;

                if (element is StationViewModel)
                {
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                    ((StationImpl) element.Element).Color = "Blue";
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                }
                
                // Should show the StationData usercontrol



            }
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
            String name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the station", "Add station", "Default", -1, -1);
            undoAndRedoController.AddToStackAndExecute(new AddStationCommand(iModel, name, 20, 20));
            //iModel.AddStation(name, 20, 20);
            RefreshElements();
        }
        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var Element in iModel.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(Element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(elementViewModel);
            }
        }
    }
}