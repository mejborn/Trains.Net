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
        private IModel _iModel;
        private string _fileName;
        private BaseElementViewModel _selectedElement;

        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand AddNodeCommand => new RelayCommand(AddNode);
        public ICommand AddStationCommand => new RelayCommand(AddStation);
        public ICommand AddConnectionCommand => new RelayCommand(AddConnection);

        private void AddConnection()
        {
            var station1 = Elements[0].Element as StationImpl;
            var station2 = Elements[1].Element as StationImpl;
            _iModel.ConnectNodes(station1,station2);
            RefreshElements();
        }

        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);
        public ICommand AddConnectionPointCommand => new RelayCommand<string>(v =>
        {
            (_selectedElement as StationViewModel)?.AddConnectionPoint(v); RefreshElements();
        } );
        private UndoAndRedoController UndoAndRedoController => UndoAndRedoController.instanceOfUndoRedo;

        public ICommand UndoOperation => UndoAndRedoController.UndoCommand;
        public ICommand RedoOperation => UndoAndRedoController.RedoCommand;

        public void ShowSaveDialog()
        {
            var sfd = new SaveFileDialog();
            sfd.ShowDialog();
            _fileName = sfd.FileName;
        }
        public void ShowLoadDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            _fileName = ofd.FileName;
        }

        public void SaveModel()
        {
            if (string.IsNullOrEmpty(_fileName))
                ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_iModel,_fileName);
        }
        public void SaveModelAs()
        {
            ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_iModel, _fileName);
        }
        public void LoadModel()
        {
            ShowLoadDialog();
            if (!string.IsNullOrEmpty(_fileName))
            _iModel = FileIOUtils.LoadObject<ModelImpl>(_fileName);
            RefreshElements();
        }
        public string InputText { get; private set; }

        public MainViewModel()
        {
            _iModel = new ModelImpl();
            foreach (var element in _iModel.GetElements())
            {
                var viewModel = Util.CreateViewModel(element);
                viewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(viewModel);
            }
        }

        private void OnHasBeenSelected(object sender, EventArgs e)
        {
            BaseElementViewModel element = sender as BaseElementViewModel;
            if (element != null)
            {
                if (_selectedElement is StationViewModel)
                {
                    ((StationImpl)_selectedElement.Element).Opacity = 1;
                }

                
                _selectedElement = element;

                if (element is StationViewModel)
                {
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                    ((StationImpl) element.Element).Opacity = 0.5;
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                }
                RefreshElements();
                // Should show the StationData usercontrol
            }
            else
                throw new NotImplementedException();
        }

        private void AddNode()
        {
            _iModel.AddNode(10,10);
            RefreshElements();
        }

        private void AddStation()
        {
            var name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the station", "Add station", "Default", -1, -1);
            UndoAndRedoController.AddToStackAndExecute(new AddStationCommand(_iModel, name, 20, 20));
            //iModel.AddStation(name, 20, 20);
            RefreshElements();
        }
        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var element in _iModel.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(elementViewModel);
            }
        }
    }
}