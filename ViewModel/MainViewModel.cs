using GalaSoft.MvvmLight;
using Model;
using Model.Elements.Interface;
using Model.Elements.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using System.Linq;
using Utility;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IModel _model;
        private string _fileName;
        private BaseElementViewModel _selectedElement;

        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand AddNodeCommand => new RelayCommand(AddNode);
        public ICommand AddStationCommand => new RelayCommand(AddStation);
        public ICommand AddConnectionCommand => new RelayCommand(AddConnection);
        public ICommand DeleteElementCommand => new RelayCommand(DeleteElement);
        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);

        public ICommand AddConnectionPointCommand => new RelayCommand<string>(v =>
        {
            var station = _selectedElement as StationViewModel;

            try
            {
                station?.AddConnectionPoint(v);
                RefreshElements();
            }
            catch (Exception e)
            {
                if (!(e is System.NullReferenceException))
                {
                    MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Please select a station first", "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        });

        private static UndoAndRedoImpl UndoAndRedoInstance => UndoAndRedoImpl.UndoAndRedoInstance;

        public RelayCommand UndoOperation => new RelayCommand(Undo);
        public RelayCommand RedoOperation => new RelayCommand(Redo);

        private bool _canUndo;
        private bool _canRedo;

        public bool CanUndo { get { return _canUndo; } set { _canUndo = value; RaisePropertyChanged(); }}
        public bool CanRedo { get { return _canRedo; } set { _canRedo = value; RaisePropertyChanged(); } }

        private void AddConnection()
        {
            try
            {
                var station1 = Elements[0].Element as StationImpl;
                var station2 = Elements[1].Element as StationImpl;
                _model.ConnectNodes(station1, station2);
                RefreshElements();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteElement()
        {
            var element = _selectedElement?.Element;
            var station = element as StationImpl;
            if (station?.ConnectionPoints.Any() == true)
                if (MessageBox.Show(
                        "Warning",
                        "This station has connections. Are you sure?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            if(element != null)
                _model.DeleteObject(element);
            RefreshElements();
        }

        public void ShowSaveDialog()
        {
            var sfd = new SaveFileDialog();
            sfd.ShowDialog();
            _fileName = sfd.FileName;
        }

        public void ShowLoadDialog()
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            _fileName = ofd.FileName;
        }

        public void SaveModel()
        {
            if (string.IsNullOrEmpty(_fileName))
                ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_model,_fileName);
        }

        public void SaveModelAs()
        {
            ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_model, _fileName);
        }

        public void LoadModel()
        {
            ShowLoadDialog();
            if (!string.IsNullOrEmpty(_fileName))
            _model = FileIOUtils.LoadObject<ModelImpl>(_fileName);
            RefreshElements();
        }
        public string InputText { get; private set; }

        public MainViewModel()
        {
            _model = new ModelImpl();
            foreach (var element in _model.GetElements())
            {
                var viewModel = Util.CreateViewModel(element);
                viewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(viewModel);
            }
        }

        private void OnHasBeenSelected(object sender, EventArgs e)
        {
            var element = sender as BaseElementViewModel;
            if (_selectedElement != null)
                _selectedElement.Opacity = 1;

            if (element != null)
                element.Opacity = 0.5;

            _selectedElement = element;
        }

        private void AddNode()
        {
            try
            {
                _model.AddNode(10, 10);
                RefreshElements();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void AddStation()
        {

            try
            {
                var name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the station",
                "Add station", "Default", -1, -1);
                UndoAndRedoInstance.AddUndoItem<string>(_model.AddStation(name, 20, 20),UndoAndRedoImpl.Actions.Insert, null);               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            RefreshElements();
        }

        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var element in _model.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(elementViewModel);
            }
            CanUndo = UndoAndRedoInstance.CanUndo;
            CanRedo = UndoAndRedoInstance.CanRedo;
        }


        private void Undo()
        {
            //CanUndo = undoAndRedoInstance.IsUndoListPopulated;
           // UndoOperation.CanExecute(CanUndo);
            //UndoAndRedoInstance.UndoOperation();
            // CommandManager.InvalidateRequerySuggested();

            // UndoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            //RedoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH

            UndoAndRedoInstance.Undo();
            RefreshElements();
        }

        private void Redo()
        {
            //CanRedo = undoAndRedoInstance.IsRedoListPopulated;
            //UndoOperation.CanExecute(CanRedo);
            //UndoAndRedoInstance.RedoOperation();
            //CommandManager.InvalidateRequerySuggested();
            //UndoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            //RedoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            RefreshElements();
        }
    }
}