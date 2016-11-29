using GalaSoft.MvvmLight;
using Model;
using Model.Elements.Interface;
using Model.Elements.Implementation;
using ViewModel.UndoAndRedo;
using ViewModel.UndoAndRedo.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using Utility;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using Model.Elements;

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

        public ICommand AddConnectionPoint => new RelayCommand<string>(v =>
        {
            var station = _selectedElement as IStation;

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

        private UndoAndRedoImpl undoAndRedoInstance => UndoAndRedoImpl.UndoAndRedoInstance;

        public RelayCommand UndoOperation;

        public RelayCommand RedoOperation;

        private bool _canUndo = false;
        private bool _canRedo = false;

        
      private bool CanUndoFunction() => CanUndo;
      private bool CanRedoFunction() => CanRedo;

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                _canUndo = value;
                RaisePropertyChanged();
            }
        }

        public bool CanRedo
        {
            get { return _canRedo; }
            set
            {
                _canRedo = value;
                RaisePropertyChanged();
            }
        }

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
            var vmElement = _selectedElement;
            try
            {

                IBaseElement element = vmElement.Element;
                
                if (element is StationImpl)
                {
                    _model.DeleteStation((StationImpl)element, false); // bool for exception throw, returnere element i exception?
                } else if (element is BaseNodeImpl)
                {
                    _model.DeleteNode((BaseNodeImpl)element);
                } else if (element is ConnectionPointImpl)
                {
                    _model.DeleteConnection((BaseConnectionImpl)element); 
                } else if (element is ConnectionPointImpl)
                {
                    _model.DeleteConnectionPoint((ConnectionPointImpl) element);//HMMM
                }
                
                RefreshElements();
            }
            catch (Exception e)
            {
                if (!(e is System.NullReferenceException))
                {
                    IBaseElement element = _selectedElement.Element;
                    if (element is StationImpl)
                    {
                        DialogResult dialogResult = MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dialogResult == DialogResult.Yes)
                        {
                            _model.DeleteStation((StationImpl) element, true);
                            RefreshElements();
                        }
                    }
                    else MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Please select a station first", "An error has occured", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
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
        public StationInfoViewModel StationInfo { get; private set; }

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

            Elements.Remove(StationInfo);
            StationInfo = null;
            if (_selectedElement != null)
                _selectedElement.Opacity = 1;

            if (element != null)
            {
                element.Opacity = 0.5;
                if (sender is StationViewModel)
                {
                    var stationVM = sender as StationViewModel;
                    var info = _model.StationInfo(stationVM.Station);

                    StationInfo = new StationInfoViewModel(info);
                    //var StationInfoViewModel = Util.CreateViewModel(info);
                    //StationInfoViewModel.HasBeenSelected += OnHasBeenSelected;
                    Elements.Add(StationInfo);
                }
            }
                

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
                undoAndRedoInstance.AddToListAndExecute(new AddStationCommand(_model, name, 200, 200));
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
            
            //CanUndo = undoAndRedoInstance.IsUndoListPopulated;
            //UndoOperation.CanExecute(CanUndo);
            //UndoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
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
        }


        private void Undo()
        {
            //CanUndo = undoAndRedoInstance.IsUndoListPopulated;
           // UndoOperation.CanExecute(CanUndo);
            undoAndRedoInstance.UndoOperation();
            // CommandManager.InvalidateRequerySuggested();

            // UndoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            //RedoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            RefreshElements();


        }

        private void Redo()
        {
            //CanRedo = undoAndRedoInstance.IsRedoListPopulated;
            //UndoOperation.CanExecute(CanRedo);
            undoAndRedoInstance.RedoOperation();
            //CommandManager.InvalidateRequerySuggested();
            //UndoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            //RedoOperation.RaiseCanExecuteChanged(); UDKOMMENTERET, DA CRASH
            RefreshElements();
        }
    }
}