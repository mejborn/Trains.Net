using GalaSoft.MvvmLight;
using Model;
using Model.Elements.Interface;
using Model.Elements.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Utility;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using GalaSoft.MvvmLight.Command;
using Model.Elements;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private variables
        private IModel _model;
        private string _fileName;
        private BaseElementViewModel _selectedElement;
        private BaseElementViewModel _oldSelectedElement;
        private ConnectionListViewModel _listViewModel;
        private BaseElementViewModel _elementCopy;
        private bool _canUndo;
        private bool _canRedo;
        private bool _canCopy;
        private bool _canPaste;
        private bool _isAddConnectionPressedSuccesfully;
        private static UndoAndRedoImpl UndoAndRedoInstance => UndoAndRedoImpl.UndoAndRedoInstance;
        #endregion

        #region Public properties
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ObservableCollection<StationViewModel> Stations { get; } = new ObservableCollection<StationViewModel>();
        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();
        public ObservableCollection<StationInfoViewModel> Info { get; } = new ObservableCollection<StationInfoViewModel>();
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
        public ICommand PrintCommand => new RelayCommand<object>(Print);
        public ICommand NewCommand => new RelayCommand(NewModel);

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

        private void NewModel()
        {
            if (
                MessageBox.Show("Are you sure???", "Delete current project?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            _model = new ModelImpl();
            RefreshElements();
            RefreshButtons();
        }
        private void Print(object o)
        {
            if(!(o is Window))
                return;
            var sfd = new SaveFileDialog()
            {
                Filter = "Xps files | *.xps",
                DefaultExt = "*.x"
            };
            if(sfd.ShowDialog() != DialogResult.OK)
                return;
            _fileName = sfd.FileName;

            var lMemoryStream = new MemoryStream();
            var package = Package.Open(lMemoryStream, FileMode.Create);
            var writer = XpsDocument.CreateXpsDocumentWriter(new XpsDocument(package));
            writer.Write((Window) o);
            package.Close();
            lMemoryStream.WriteTo(new FileStream(_fileName, FileMode.Create, FileAccess.Write));
        }
        private void Cut()
        {
            if (!CanCopy)
                return;
            _elementCopy = _selectedElement;
            DeleteElement();
            RefreshButtons();
        }

        private void Copy()
        {
            if (!CanCopy)
                return;
            _elementCopy = _selectedElement.ShallowCopy();
            RefreshButtons();
        }

        private void Paste()
        {
            if (!CanPaste)
                return;
            if (_elementCopy is StationViewModel)
            {
                try
                {
                    var oldStation = _elementCopy.Element as IStation;
                    var oldName = oldStation.Name;
                    var name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the new station",
                        "Copy station", oldName, -1, -1);
                    var element = _model.AddStation(name, oldStation.Left, oldStation.Top);
                    var vm = Util.CreateViewModel(element);
                    vm.HasBeenReleased += OnHasBeenReleased;
                    vm.HasBeenSelected += OnHasBeenSelected;
                    UndoAndRedoInstance.AddUndoItem<string>(vm, UndoAndRedoImpl.Actions.Insert, null);
                    Elements.Add(vm);
                    Stations.Add(vm as StationViewModel);
                    foreach (var station in _model.GetStationsConnectedToNode(oldStation))
                    {
                        if (station.Equals(oldStation)) continue;
                        foreach (var viewModel in Elements)
                        {
                            if (viewModel.Element.Equals(station))
                            {
                                _oldSelectedElement = viewModel;
                                break;
                            }
                        }
                        _selectedElement = vm;
                        AddConnection();
                    }
                    RefreshButtons();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            } else
            {
                _model.AddElement(_elementCopy?.Element as BaseElementImpl);
                Elements.Add(_elementCopy);
            }
            var stvm = _elementCopy as StationViewModel;
            //if (stvm != null)
            //    Stations.Add(_elementCopy as StationViewModel);
            RefreshButtons();
        }
        private void AddLine()
        {
            try
            {
                var name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the line",
                    "Add line", "Default", -1, -1);
                var station1 = _oldSelectedElement.Element as IStation;
                var station2 = _selectedElement.Element as IStation;
                var stations = _model.CreateLine(name, station1, station2);
                var LineViewModel = new LineViewModel(name, stations);
                Lines.Add(LineViewModel);
                
                RefreshButtons();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddConnection()
        {
            try
            {
                /*if (!_isAddConnectionPressedSuccesfully)
                {
                    MessageBox.Show("Please select the other node or station you want to connect to","Note")
                    
                }*/

                if (_selectedElement == null || _oldSelectedElement == null || _oldSelectedElement == _selectedElement)
                {
                    MessageBox.Show("You must select a station or node i order to connect them!", "Error!");
                    return;
                }
                
                NodeViewModel vmNode1 = null;
                NodeViewModel vmNode2 = null;
                StationViewModel vmStation1 = null;
                StationViewModel vmStation2 = null;

                BaseNodeImpl node1 = null;
                BaseNodeImpl node2 = null;
                StationImpl station1 = null;
                StationImpl station2 = null;

                BaseConnectionImpl element = null;

                if (_oldSelectedElement is StationViewModel && _selectedElement is StationViewModel)
                {
                    vmStation1 = _oldSelectedElement as StationViewModel;
                    vmStation2 = _selectedElement as StationViewModel;
                    station1 = vmStation1?.Station as StationImpl;
                    station2 = vmStation2?.Station as StationImpl;

                    var positions = ConnectionPointPositionToNode(vmStation1, vmStation2);
                    var cp1 = (vmStation1)?.AddConnectionPoint(positions[0]);
                    var cp2 = (vmStation2)?.AddConnectionPoint(positions[1]);

                    element = _model.ConnectNodes(station1, station2, cp1, cp2);
                    cp1.Connection = element;
                    cp2.Connection = element;

                } else if (_oldSelectedElement is StationViewModel && _selectedElement is NodeViewModel)
                {
                    vmStation1 = _oldSelectedElement as StationViewModel;
                    vmNode2 = _selectedElement as NodeViewModel;
                    station1 = vmStation1?.Element as StationImpl;
                    node2 = vmNode2?.Element as BaseNodeImpl;

                    var positions = ConnectionPointPositionToNode(vmStation1, vmNode2);
                    var cp1 = (vmStation1)?.AddConnectionPoint(positions[0]);
                    
                    element = _model.ConnectNodes(station1, node2, cp1, null);
                    cp1.Connection = element;

                } else if (_oldSelectedElement is NodeViewModel && _selectedElement is StationViewModel)
                {
                    vmNode1 = _oldSelectedElement as NodeViewModel;
                    vmStation2 = _selectedElement as StationViewModel;

                    node1 = vmNode1?.Element as BaseNodeImpl;
                    station2 = vmStation2?.Element as StationImpl;

                    var positions = ConnectionPointPositionToNode(vmNode1, vmStation2);
                    var cp2 = (vmStation2)?.AddConnectionPoint(positions[1]);

                    element = _model.ConnectNodes(node1, station2, null, cp2);
                    cp2.Connection = element;
                }
                else
                {
                    vmNode1 = _oldSelectedElement as NodeViewModel;
                    vmNode2 = _selectedElement as NodeViewModel;
                    node1 = vmNode1?.Element as BaseNodeImpl;
                    node2 = vmNode2?.Element as BaseNodeImpl;
                    
                    element = _model.ConnectNodes(node1, node2, null, null);
                }
                
                var vm = Util.CreateViewModel(element);
                
                if (_oldSelectedElement is StationViewModel && _selectedElement is StationViewModel)
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

            List<NodeViewModel> otherVMNodes = new List<NodeViewModel>();

            if (elementViewModel is StationViewModel)
            {
                var connections = station?.Connections;

                foreach (var connection in connections)
                {
                    foreach (var vmConnection in Elements)
                    {
                        if ((vmConnection is BaseConnectionViewModel) && (vmConnection as BaseConnectionViewModel).Element == connection)
                        {
                            Elements.Remove(vmConnection);

                            break;
                        }
                    }

                    if (connection.node1 == (elementViewModel.Element as StationImpl))
                    {
                        foreach (var vmStation in Elements)
                        {
                            if (vmStation is StationViewModel && vmStation.Element == connection.node2)
                            {
                                otherVMNodes.Add(vmStation as StationViewModel);
                                //(vmStation as StationViewModel).ConnectionPoints.Remove()
                            }
                        }
                    }
                    else {
                        foreach (var vmStation in Elements)
                        {
                            if (vmStation is StationViewModel && vmStation.Element == connection.node1)
                            {
                                otherVMNodes.Add(vmStation as StationViewModel);
                            }
                        }
                    }
                }
            }

            _model.DeleteObject(elementViewModel.Element);

            foreach (var cp in Elements)
            {
                //if(cp is ConnectionPointViewModel && (cp ConnectionPointViewModel).)

            }

            Elements.Remove(elementViewModel);
            Stations.Remove(elementViewModel as StationViewModel);
            if (Stations.Count == 0)
            {
                Info.Remove(StationInfo);
                StationInfo = null;
            }

            foreach (var vmStation in otherVMNodes)
            {
                if (_model.GetStationsConnectedToNode(vmStation.BaseNode).Count == 0)
                {
                    vmStation.Color = "Red";
                }
                (vmStation as StationViewModel).UpdateConnectionPointPositions();
            }

            UndoAndRedoInstance.AddUndoItem(elementViewModel, UndoAndRedoImpl.Actions.Remove, "");
            _selectedElement = _oldSelectedElement;
        }

        public void ShowSaveDialog()
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "Xml files | *.xml",
                DefaultExt = "*.xml"
            };
            sfd.ShowDialog();
            _fileName = sfd.FileName;
        }

        public void ShowLoadDialog()
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "Xml files | *.xml",
                DefaultExt = "*.xml"
            };
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
           /* if (_isAddConnectionPressedSuccesfully)
            {
                AddConnection(_selectedElement, sender);
            }
             */   

            var element = sender as BaseElementViewModel;

            Info.Remove(StationInfo);
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
                    //var info = _model.StationInfo(station);
                    StationInfo = new StationInfoViewModel(station, _model);

                    //StationInfo.updateConnections();
                    Info.Add(StationInfo);
                }
            }

            if (_oldSelectedElement == null)
            {
                _oldSelectedElement = _selectedElement;
            }
            else if (_selectedElement != element)
            {
                _oldSelectedElement = _selectedElement;
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
            Stations.Clear();
            Elements.Clear();
            foreach (var element in _model.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                elementViewModel.HasBeenReleased += OnHasBeenReleased;
                Elements.Add(elementViewModel);
                Stations.Add(elementViewModel as StationViewModel);
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
            if (!CanUndo)
                return;
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
            if (!CanRedo)
                return;
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

        private string[] ConnectionPointPositionToNode(NodeViewModel vmNode1, NodeViewModel vmNode2)
        {
            
            var node1 = vmNode1.Element;
            var node2 = vmNode2.Element;
            string[] position = new string[2]; // First index, i.e. 0, is for first station...


            
           Point centerNode1 = new Point(node1.Left + (node1.Width/2), node1.Top + (node1.Height / 2));
           Point centerNode2 = new Point(node2.Left + (node2.Width / 2), node2.Top + (node2.Height / 2));

           double horisontalDistance = centerNode2.X - centerNode1.X;
           double verticalDistance = centerNode2.Y - centerNode1.Y;

           double radAngleToXAxis = Math.Atan(Math.Abs(verticalDistance)/ Math.Abs(horisontalDistance));
            
            position[1] = (horisontalDistance >= 0 && verticalDistance <= 0 ) ?
                                (radAngleToXAxis >= (Math.PI / 4)) ? "Bottom" : "Left" :
                          (horisontalDistance >= 0 && verticalDistance >= 0) ?
                                (radAngleToXAxis > (Math.PI / 4)) ? "Top" : "Left" :
                          (horisontalDistance <= 0 && verticalDistance >= 0) ?
                                (radAngleToXAxis > (Math.PI / 4)) ? "Top" : "Right" :
                                (radAngleToXAxis > (Math.PI / 4)) ? "Bottom" : "Right";
            
        
            position[0] = (position[1] == "Top") ? "Bottom" :
                          (position[1] == "Bottom") ? "Top" :
                          (position[1] == "Left") ? "Right" : "Left";

            Console.WriteLine("1st: " + position[0]);
            Console.WriteLine("2nd: " + position[1]);

            
            //(Math.Atan(Math.Abs((station1.Left + station1.Width/2) - (station2.Left + station2.Width/2))) <= Math.PI/4)? "Right"  : "";

            return position;
        }
        #endregion
    }
}