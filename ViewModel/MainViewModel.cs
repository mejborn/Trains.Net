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
using System.Windows;
using Utility;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GalaSoft.MvvmLight.Command;
using Model.Elements;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private variables
        private IModel _model;
        private string _fileName;
        private BaseElementViewModel _selectedElement;
        private ConnectionListViewModel _listViewModel;
        private BaseElementViewModel _elementCopy;
        private bool _canUndo;
        private bool _canRedo;
        private bool _canCopy;
        private bool _canPaste;
        private static UndoAndRedoImpl UndoAndRedoInstance => UndoAndRedoImpl.UndoAndRedoInstance;
        #endregion

        #region Public properties
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ObservableCollection<StationViewModel> Stations { get; } = new ObservableCollection<StationViewModel>();
        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();
        public bool CanUndo { get { return _canUndo; } set { _canUndo = value; RaisePropertyChanged(); } }
        public bool CanRedo { get { return _canRedo; } set { _canRedo = value; RaisePropertyChanged(); } }
        public bool CanCopy { get { return _canCopy;} set { _canCopy = value; RaisePropertyChanged(); } }
        public bool CanPaste { get { return _canPaste; } set { _canPaste = value; RaisePropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand AddNodeCommand => new RelayCommand(AddNode);
        public ICommand AddStationCommand => new RelayCommand(AddStation);
        public ICommand AddConnectionCommand => new RelayCommand(AddConnection);
        public ICommand DeleteElementCommand => new RelayCommand(DeleteElement);
        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);
        public ICommand AddLineCommand => new RelayCommand(AddLine);
        public ICommand CutCommand => new RelayCommand(Cut);
        public ICommand CopyCommand => new RelayCommand(Copy);
        public ICommand PasteCommand => new RelayCommand(Paste);
        public RelayCommand UndoOperation => new RelayCommand(Undo);
        public RelayCommand RedoOperation => new RelayCommand(Redo);

        public ICommand AddConnectionPointCommand => new RelayCommand<string>(AddConnectionPoint);
        
        #endregion

        #region Private methods
        private void AddConnectionPoint(string v)
        {
            var station = _selectedElement as StationViewModel;

            try
            {
                station?.AddConnectionPoint(v);
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a station first", "An error has occured", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Private methods
        private void Cut()
        {
            _elementCopy = _selectedElement;
            DeleteElement();
            RefreshButtons();
        }

        private void Copy()
        {
            _elementCopy = _selectedElement.ShallowCopy();
            RefreshButtons();
        }

        private void Paste()
        {
            _model.AddElement(_elementCopy?.Element as BaseElementImpl);
            Elements.Add(_elementCopy);
            var stvm = _elementCopy as StationViewModel;
            if (stvm != null)
                Stations.Add(_elementCopy as StationViewModel);
            RefreshButtons();
        }
        private void AddLine() { Console.WriteLine("Not implementet"); }

        private void AddConnection()
        {
            try
            {

                var vmStation1 = Elements[0] as NodeViewModel;
                var vmStation2 = Elements[1] as NodeViewModel;

                var station1 = Elements[0].Element as StationImpl;
                var station2 = Elements[1].Element as StationImpl;
                var positions = PositionToNode(Elements[0] as StationViewModel, Elements[1] as StationViewModel);

                var cp1 = (vmStation1 as StationViewModel)?.AddConnectionPoint(positions[0]);
                var cp2 = (vmStation2 as StationViewModel)?.AddConnectionPoint(positions[1]);
                
                var element =_model.ConnectNodes(station1, station2, cp1, cp2);
                var vm = Util.CreateViewModel(element);
                
                if (vmStation1 is StationViewModel && vmStation2 is StationViewModel)
                {
                    vmStation1.Color = "Green"; //Skal måske ændres ift. at noder laves automatisk ved connection af 2 station
                    vmStation2.Color = "Green";
                }
                    
                vm.HasBeenReleased += OnHasBeenReleased;
                vm.HasBeenSelected += OnHasBeenSelected;
                UndoAndRedoInstance.AddUndoItem<string>(vm, UndoAndRedoImpl.Actions.Insert, null);
                // Mangler også Connection Point her
                Elements.Add(vm);
                RefreshButtons();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteElement()
        {
            var elementViewModel = _selectedElement;
            var station = elementViewModel.Element as StationImpl;
            if (station?.Connections.Any() == true)
                if (MessageBox.Show(
                        "This station has connections. Are you sure?",
                        "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            /*
            if (elementViewModel is BaseConnectionViewModel)
            {
                _model.GetStationsConnectedToNode((elementViewModel as BaseConnectionViewModel).
                    
                    
                    .Count == 0)
            }
            if (GetStationsConnectedToNode(node1).Count == 0) node1.Color = "Red";
            if (GetStationsConnectedToNode(node2).Count == 0) node2.Color = "Red";
            */
            _model.DeleteObject(elementViewModel.Element);
            Elements.Remove(elementViewModel);
            Stations.Remove(elementViewModel as StationViewModel);
            UndoAndRedoInstance.AddUndoItem(elementViewModel, UndoAndRedoImpl.Actions.Remove, "");
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
                FileIOUtils.SaveObject(_model, _fileName);
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
                    var station = stationVM.Station;
                    var info = _model.StationInfo(station);
                    StationInfo = new StationInfoViewModel(info);
                    
                    foreach (var s in _model.GetStationsConnectedToNode(station))
                    {
                        StationInfo.Connections.Add(s.Name);
                    }
                    Elements.Add(StationInfo);
                }
            }
                
            _selectedElement = element;
            RefreshButtons();
        }

        private void OnHasBeenReleased(object sender, EventArgs e)
        {
            var element = sender as BaseElementViewModel;
            if (element != null &&
                Math.Abs(element.Top - element.PrevPos.Y) > 0.0001 &&
                Math.Abs(element.Left - element.PrevPos.X) > 0.0001)
            {
                UndoAndRedoInstance.AddUndoItem(element, UndoAndRedoImpl.Actions.Move, element.PrevPos);

                if (element is NodeViewModel)
                {
                    var node = (element as NodeViewModel).BaseNode;

                    foreach (var connection in node.Connections)
                    {
                        foreach (var vmElement in Elements)
                        {
                            if (vmElement is BaseConnectionViewModel && (vmElement as BaseConnectionViewModel).Element == connection)
                            {
                                CalculateNewConnectionPos(vmElement as BaseConnectionViewModel, element as NodeViewModel);
                                break;


                            }
                        }
                        
                    }
                    
                }
            }
        }

        private void AddNode()
        {
            try
            {
                var node = _model.AddNode(10, 10);
                var vm = Util.CreateViewModel(node);
                vm.HasBeenReleased += OnHasBeenReleased;
                vm.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(vm);
                UndoAndRedoInstance.AddUndoItem(vm,UndoAndRedoImpl.Actions.Insert, "");
                RefreshButtons();
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
                var element = _model.AddStation(name, 20, 20);
                var vm = Util.CreateViewModel(element);
                vm.HasBeenReleased += OnHasBeenReleased;
                vm.HasBeenSelected += OnHasBeenSelected;
                UndoAndRedoInstance.AddUndoItem<string>(vm, UndoAndRedoImpl.Actions.Insert, null);
                Elements.Add(vm);
                Stations.Add(vm as StationViewModel);
                RefreshButtons();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButtons()
        {
            CanCopy = _selectedElement != null;
            CanPaste = _elementCopy != null;
            CanUndo = UndoAndRedoInstance.CanUndo;
            CanRedo = UndoAndRedoInstance.CanRedo;
        }

        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var element in _model.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                elementViewModel.HasBeenReleased += OnHasBeenReleased;
                Elements.Add(elementViewModel);
            }
            CanUndo = UndoAndRedoInstance.CanUndo;
            CanRedo = UndoAndRedoInstance.CanRedo;
        }

        private void UpdateElementPosition(BaseElementViewModel e, Point p)
        {
            var prevPos = new Point(e.Left, e.Top);
            e.Top = p.Y;
            e.Left = p.X;
            e.PrevPos = prevPos;
        }

        private void Undo()
        {
            var element = UndoAndRedoInstance.Undo() as IUndoRedoObject;
            var vm = element?.O as BaseElementViewModel;
            switch (element?.A)
            {
                case UndoAndRedoImpl.Actions.Insert:
                    _model.RemoveElement(vm?.Element as BaseElementImpl);
                    Elements.Remove(vm);
                    Stations.Remove(vm as StationViewModel);
                    break;
                case UndoAndRedoImpl.Actions.Remove:
                    _model.AddElement(vm?.Element as BaseElementImpl);
                    Elements.Add(vm);
                    var stvm = vm as StationViewModel;
                    if(stvm != null)
                        Stations.Add(vm as StationViewModel);
                    break;
                case UndoAndRedoImpl.Actions.Move:
                    var moveObject = element as UndoRedoObject<Point>;
                    if (moveObject != null)
                        foreach (var viewModel in Elements)
                        {
                            if (viewModel == vm)
                                UpdateElementPosition(viewModel, vm.PrevPos);
                        }
                    break;
                case UndoAndRedoImpl.Actions.Update:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RefreshButtons();
        }

        private void Redo()
        {
            var element = UndoAndRedoInstance.Redo() as IUndoRedoObject;
            var vm = element?.O as BaseElementViewModel;
            switch (element?.A)
            {
                case UndoAndRedoImpl.Actions.Insert:
                    _model.AddElement(vm?.Element as BaseElementImpl);
                    Elements.Add(vm);
                    var stvm = vm as StationViewModel;
                    if(stvm != null)
                        Stations.Add(vm as StationViewModel);
                    break;
                case UndoAndRedoImpl.Actions.Remove:
                    _model.RemoveElement(vm?.Element as BaseElementImpl);
                    Elements.Remove(vm);
                    Stations.Remove(vm as StationViewModel);
                    break;
                case UndoAndRedoImpl.Actions.Move:
                    var moveObject = element as UndoRedoObject<Point>;
                    if (moveObject != null)
                        foreach (var viewModel in Elements)
                        {
                            if (viewModel == vm)
                                UpdateElementPosition(viewModel, vm.PrevPos);
                        }
                    break;
                case UndoAndRedoImpl.Actions.Update:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RefreshButtons();
        }

        private void CalculateNewConnectionPos(BaseConnectionViewModel vmConnection, NodeViewModel vmNode)
        {
            var connection = vmConnection.Element as BaseConnectionImpl;

            if (connection?.node1 == vmNode.BaseNode)
            {
                vmConnection.X1 = vmNode.Left + connection.CP1.Left + (connection.CP1.Width / 2);
                vmConnection.Y1 = vmNode.Top + connection.CP1.Top + (connection.CP1.Height / 2);
            }
            else
            {
                vmConnection.X2 = vmNode.Left + connection.CP2.Left + (connection.CP2.Width / 2);
                vmConnection.Y2 = vmNode.Top + connection.CP2.Top + (connection.CP2.Height / 2);
            }


        }

        private string[] PositionToNode(StationViewModel vmStation1, StationViewModel vmStation2)
        {
            
            var station1 = vmStation1.Element;
            var station2 = vmStation2.Element;
            string[] position = new string[2];

            position[0] = (station1.Top >= station2.Top) ? "Top" :
                          (station1.Left <= station2.Left) ? "Right" :
                          (station1.Top < station2.Top) ? "Bottom" : "Left";

            position[1] = (position[0] == "Top") ? "Bottom" :
                          (position[0] == "Bottom") ? "Top" :
                          (position[0] == "Left") ? "Right" : "Left";
            
            //(Math.Atan(Math.Abs((station1.Left + station1.Width/2) - (station2.Left + station2.Width/2))) <= Math.PI/4)? "Right"  : "";

            return position;
        }
        #endregion
    }
}